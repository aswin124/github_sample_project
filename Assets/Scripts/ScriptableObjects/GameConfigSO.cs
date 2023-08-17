using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Card.Data
{
    /// <summary>
    /// We need to mention all the game related config details here,
    /// Later we can use it as a asset bundle to update our changes in the app without releasing a new build.
    /// </summary>
    [CreateAssetMenu(fileName ="GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfigSO : ScriptableObject
    {

        [SerializeField] private float cardFlipSpeed;
        public float CardFlipSpeed
        {
            set { CardFlipSpeed = value; }
            get { return CardFlipSpeed; }
        }

        [SerializeField] private float cardInitialViewTime;
        public float CardInitialViewTime
        {
            set { cardInitialViewTime = value; }
            get { return cardInitialViewTime; }
        }

        [SerializeField] private List<LevelDataSO> levels;
        public List<LevelDataSO> Levels
        {
            get { return levels; }
        }
        //Audio SO
    }
}
