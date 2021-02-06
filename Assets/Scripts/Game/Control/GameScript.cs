using System;
using System.Linq;
using Common.Extensions;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Control
{
    public class GameScript : Singleton<GameScript>
    {
        [SerializeField] private EndGameWindow endGame;

        private void Start()
        {
            Managers.GameControl.OnWin += delegate
            {
                endGame.Active(true);
            };
            
            Managers.GameControl.OnLose += delegate
            {
                endGame.Active(false);
            };
        }

        [Serializable]
        private class EndGameWindow
        {
            [SerializeField] private GameObject endWindow;

            [SerializeField] private GameObject[] winActive;

            [SerializeField] private GameObject[] loseActive;

            [SerializeField] private Image backImage;

            [SerializeField] private Sprite winSprite;

            [SerializeField] private Sprite loseSprite;

            public void Active(bool isWin)
            {
                endWindow.SetActive(true);
                
                winActive.ToList().ForEach(x => x.SetActive(isWin));
                
                loseActive.ToList().ForEach(x => x.SetActive(!isWin));

                backImage.sprite = isWin ? winSprite : loseSprite;
            }
        }
    }
}