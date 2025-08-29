using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.Gui.NamePlate;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using LightlessSync.LightlessConfiguration;
using LightlessSync.PlayerData.Pairs;
using LightlessSync.Services.Mediator;
using LightlessSync.UI;
using Microsoft.Extensions.Logging;
namespace LightlessSync.Services;

public class NameplateService : DisposableMediatorSubscriberBase
{
    private readonly LightlessConfigService _configService;
    private readonly IClientState _clientState;
    private readonly INamePlateGui _namePlateGui;
    private readonly PairManager _pairManager;

    public NameplateService(ILogger<NameplateService> logger,
        LightlessConfigService configService,
        INamePlateGui namePlateGui,
        IClientState clientState,
        PairManager pairManager,
        LightlessMediator lightlessMediator) : base(logger, lightlessMediator)
    {
        _configService = configService;
        _namePlateGui = namePlateGui;
        _clientState = clientState;
        _pairManager = pairManager;
        _namePlateGui.OnNamePlateUpdate += OnNamePlateUpdate;
        _namePlateGui.RequestRedraw();
        Mediator.Subscribe<VisibilityChange>(this, (_) => _namePlateGui.RequestRedraw());

    }

    private void OnNamePlateUpdate(INamePlateUpdateContext context, IReadOnlyList<INamePlateUpdateHandler> handlers)
    {

        if (!_configService.Current.IsNameplateColorsEnabled && !_clientState.IsPvPExcludingDen) return;
        var visibleUsersIds = _pairManager.GetOnlineUserPairs().Where(u => u.IsVisible && u.PlayerCharacterId != uint.MaxValue).Select(u => (ulong)u.PlayerCharacterId).ToHashSet();
        var colors = _configService.Current.NameplateColors;

        foreach (var handler in handlers)
        {
            var playerCharacter = handler.PlayerCharacter;
            if (playerCharacter == null) { continue; }
            var isInParty = playerCharacter.StatusFlags.HasFlag(StatusFlags.PartyMember);
            var isFriend = playerCharacter.StatusFlags.HasFlag(StatusFlags.Friend);
            bool partyColorAllowed = (_configService.Current.overridePartyColor && isInParty);
            bool friendColorAllowed = (_configService.Current.overrideFriendColor && isFriend);

            if (visibleUsersIds.Contains(handler.GameObjectId) &&
                    !(
                        (isInParty && !partyColorAllowed) ||
                        (isFriend && !friendColorAllowed)
                    ))
            {
                handler.NameParts.TextWrap = CreateTextWrap(colors);
            }
        }
    }

    public void RequestRedraw()
    {
        _namePlateGui.RequestRedraw();
    }

    private static (SeString, SeString) CreateTextWrap(DtrEntry.Colors color)
    {
        var left = new Lumina.Text.SeStringBuilder();
        var right = new Lumina.Text.SeStringBuilder();

        left.PushColorRgba(color.Foreground);
        right.PopColor();

        left.PushEdgeColorRgba(color.Glow);
        right.PopEdgeColor();

        return (left.ToReadOnlySeString().ToDalamudString(), right.ToReadOnlySeString().ToDalamudString());
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _namePlateGui.OnNamePlateUpdate -= OnNamePlateUpdate;
        _namePlateGui.RequestRedraw();
    }
}

