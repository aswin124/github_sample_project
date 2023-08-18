using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card
{
    public enum GameState
    {
        Initialize,
        MainMenu,
        GamePlay,
    }

    public enum MainMenuState
    {
        Initialize,
        Idle,
        PreviousGamePopUp,
        End 
    }

    public enum GamePlayState
    {
        NewGame,
        OldGame,
        StupUI,
        CardReveal,
        Playing,
        GameResult,
        End,
    }

    public enum SfxType
    {
        ButtonClick,
        CardFlip,
        CardPaired,
        CardNotPAired,
        GameWin,
    }
}
