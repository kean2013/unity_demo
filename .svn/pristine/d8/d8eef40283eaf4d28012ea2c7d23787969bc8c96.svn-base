using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix.Enum
{
    public enum HEventType
    {
        None,
        LoadingProgressEvent,
        NetConnectEvent,
        OnEnterGameReply,

        FightStartAttack,
        //
        ChapterUpdateStatusEvent,

        //
        TaskUpdateStateEvent,
        TaskInitListEvent,
        TaskAcceptEvent,
        TaskCommitEvent,
        Task_DisplayCompleted,
        TimelineFinishEvent,
        TaskOpenNpcMenuEvent,

        //
        CreateTemplateBuildEvent,

        //fightUI
        OnClickCellEvent,
        OnClickEntityEvent,
        OnCameraMoveEvent,
        OnHideNpcMenuEvent,
        OnShowProgressBarEvent,
        //
        NpcStatueChangeEvent,
        NetNpcInitListEvent,
        AddNetNpcEvent,

        FarmatTeam_TrainFninsh,
        FarmatTeam_RemoveArms,
        FarmatTeam_OnClickStartTrain,
        FarmatTeam_OnClickOpenFarmatTeamPanel,
        FarmatTeam_OnClickCloseFarmatTeamPanel,
        FarmatTeam_OnClickCloseFarmatAddPersonPanel,
        FarmatTeam_OnClickCardBtn,
        FarmatTeam_OnClickLockBtn,
        FarmatTeam_RenderListByChangeTask,
        FarmatTeam_RenderShowGuide,
        FarmatTeam_AllListDataChange,
        FarmatTeam_OneListItemDataChange,
        FarmatTeam_onClickUseBtn,
        FarmatTeam_RenderPanelByAllDataChange,
        FarmatTeam_RenderPanelByOneItemDataChange,
        GetTroopListTCallBack,
        TroopUseCallBack,
        TroopDeleteCallBack,
        TroopTrainCallBack,
        TroopUnlockCallBack,
        TroopLockIsOrNotCallBack,

        ArmyUnit_ClickTab,
        ArmyUnit_upgrade,
        ArmyUnit_Teardown,
        ArmyUnit_ResetBtn,
        ArmyUnit_OnClickOpenArmyUnitPanel,
        ArmyUnit_OnClickCloseArmyUnitPanel,
        ArmyUnitInfoCallBack,
        ArmyUnitUpgradeCallBack,
        ArmyUnitDisassemblyCallBack,
        ArmyUnitrestorationCallBack,
        ArmyUnit_OpenArmyUnitRankPanel,
        ArmyUnit_CloseArmyUnitRankPanel,
        ArmyUnit_Sort,
        RenderArmyUnitDetailPanel,
        FarmatTeam_onClickRemoveBtn,
        ArmyUnitOneDetailInfoCallBack,
        ArmyUnitUpdateUpgradeTips,

        Package_RefreshItem,
        Package_NewItem,
        Game_Tips,
    }

    [System.Flags]
    public enum LogLevel
    {
        Error = 1,
        Warning = 1 << 1,
        Info = 1 << 2,
    }

    [System.Flags]
    public enum LogModule
    {
        Fight = 1,
        Login = 1 << 1,
        Net = 1 << 2,
        Res = 1 << 3,
        ServerFight = 1 << 4,
        Package = 1 << 5,
        Units = 1 << 6,
        Troop = 1 << 7,
        GM = 1 << 8,
        Task = 1 << 9,
        MapEditor = 1 << 10,
        Story = 1 << 11,
        Building = 1 << 12,
    }

    public enum TipsType
    {
        Language,
        Tips,
    }

    /// <summary>
    /// 任务步骤
    /// </summary>
    public enum TaskDisplyStep
    {
        onAccept,
        onProgressing,
        onCompletable,
        onCommit,
    }


    public enum NpcRelationType
    {
        BeMoveAble = 0,
        BeAttackedAble = 1,
        BeOccupiedAble = 2,
        BeRepairedAble = 3,
        BeExploredAble = 4,
        BeUpgradedAble = 5,
        MAX,
    }

    public enum PosRelative
    {
        error = -1,
        inside,
        east,
        west,
        south,
        north,
        northEast,
        northWest,
        southEast,
        southWest,
    }
}