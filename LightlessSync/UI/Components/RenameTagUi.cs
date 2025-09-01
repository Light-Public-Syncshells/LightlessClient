using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using LightlessSync.PlayerData.Pairs;
using LightlessSync.UI.Handlers;

using System.Numerics;

namespace LightlessSync.UI.Components;

public class RenameTagUi
{
    private readonly TagHandler _tagHandler;
    private readonly UiSharedService _uiSharedService;
    private string _desiredName = string.Empty;
    private bool _opened = false;
    private HashSet<string> _peopleInGroup = new(StringComparer.Ordinal);
    private bool _show = false;
    private string _tag = string.Empty;

    public RenameTagUi(TagHandler tagHandler, UiSharedService uiSharedService)
    {
        _tagHandler = tagHandler;
        _uiSharedService = uiSharedService;
    }

    public void Draw(List<Pair> pairs)
    {
        var workHeight = ImGui.GetMainViewport().WorkSize.Y / ImGuiHelpers.GlobalScale;
        var minSize = new Vector2(300, workHeight < 110 ? workHeight : 110) * ImGuiHelpers.GlobalScale;
        var maxSize = new Vector2(300, 110) * ImGuiHelpers.GlobalScale;

        var popupName = $"Renaming Group {_tag}";

        if (!_show)
        {
            _opened = false;
        }

        if (_show && !_opened)
        {
            ImGui.SetNextWindowSize(minSize);
            UiSharedService.CenterNextWindow(minSize.X, minSize.Y, ImGuiCond.Always);
            ImGui.OpenPopup(popupName);
            _opened = true;
        }

        ImGui.SetNextWindowSizeConstraints(minSize, maxSize);
        if (ImGui.BeginPopupModal(popupName, ref _show, ImGuiWindowFlags.Popup | ImGuiWindowFlags.Modal))
        {
            ImGui.TextUnformatted($"Renaming {_tag}");

            ImGui.InputTextWithHint("##desiredname", "Enter new group name", ref _desiredName, 255, ImGuiInputTextFlags.None);
            using (ImRaii.Disabled(string.IsNullOrEmpty(_desiredName)))
            {
                if (_uiSharedService.IconTextButton(Dalamud.Interface.FontAwesomeIcon.Plus, "Rename Group"))
                {
                    RenameTag(pairs, _tag, _desiredName);
                    _show = false;
                }
            }
            ImGui.EndPopup();
        }
        else
        {
            _show = false;
        }
    }

    public void Open(string tag)
    {
        _peopleInGroup = _tagHandler.GetOtherUidsForTag(tag);
        _tag = tag;
        _desiredName = "";
        _show = true;
    }
    public void RenameTag(List<Pair> pairs, string oldTag, string newTag)
    {
        //Removal of old tag
        _tagHandler.RemoveTag(oldTag);

        //Creation of new tag and adding of old group pairs in new one.
        _tagHandler.AddTag(newTag);
        foreach (Pair pair in pairs)
        {
            var isInTag = _peopleInGroup.Contains(pair.UserData.UID);
            if (isInTag)
            {
                _tagHandler.AddTagToPairedUid(pair.UserData.UID, newTag);
            }
        }
    }
}