using System;
using System.Collections;
using Addone;
using FetchUi;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuControl : MonoBehaviour
    {
        public string sourceWebSite = "https://google.com";

        public FetchButton continueFetchButton;

        public FetchButton newGameFetchButton;

        public FetchButton optionsFetchButton;

        public FetchButton creditsFetchButton;

        public FetchButton sourceWebSiteFetchButton;

        public FetchButton[] difficultFetchButtons;

        public GameObject mainWindow;

        public GameObject difficultWindow;

        public float speedOpenDifficult = 10;

        public float speedCloseDifficult = 20;

        public Vector2 startPositionMain;

        public Vector2 selectDifficultPositionMain;

        public RectTransform hoverLight;

        public static MenuControl Instance;

        private Sprite normalNewGameFetchButtonSprite;

        private Vector2 TargetPosition { get; set; }

        private RectTransform MainWindowRect => mainWindow.GetComponent<RectTransform>();

        private Vector2 MainWindowPosition
        {
            get => MainWindowRect.anchoredPosition;

            set => MainWindowRect.anchoredPosition = value;
        }

        private bool IsOnPlace => MainWindowPosition == TargetPosition;

        private void InitFetchButtons()
        {
            continueFetchButton.Interactable = SaveVariables.firstLaunch.GetInt != 0;

            continueFetchButton.onClick.AddListener(OnContinuePressed);

            newGameFetchButton.onClick.AddListener(OnNewGamePressed);

            optionsFetchButton.onClick.AddListener(OnOptionsPressed);

            creditsFetchButton.onClick.AddListener(OnCreditsPressed);

            sourceWebSiteFetchButton.onClick.AddListener(OnSourceWebSitePressed);

            for (var i = 0; i < difficultFetchButtons.Length; i++)
            {
                var i1 = i;

                difficultFetchButtons[i].onClick.AddListener(delegate { DifficultPressed(i1); });
            }
        }

        #region EVENT BUTTONS

        private void OnContinuePressed()
        {
            SceneManager.LoadScene("game");
        }

        private void OnNewGamePressed()
        {
            if (!IsOnPlace)
                return;

            TargetPosition = TargetPosition == selectDifficultPositionMain
                ? startPositionMain
                : selectDifficultPositionMain;

            StartCoroutine(GoingToTarget());
        }

        private void OnOptionsPressed()
        {
        }

        private void OnCreditsPressed()
        {
        }

        private void OnSourceWebSitePressed()
        {
            Application.OpenURL(sourceWebSite);
        }

        private void DifficultPressed(int difficult)
        {
            Managers.Clear();

            OnContinuePressed();
        }

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            normalNewGameFetchButtonSprite = newGameFetchButton.selectedSprite == null
                ? newGameFetchButton.image.sprite
                : newGameFetchButton.selectedSprite;

            InitFetchButtons();

            mainWindow.SetActive(true);

            difficultWindow.SetActive(false);

            TargetPosition = startPositionMain;

            MainWindowPosition = startPositionMain;
        }

        private IEnumerator GoingToTarget()
        {
            var newGameSprite = newGameFetchButton.pressedSprite;

            var isGoToEnd = TargetPosition == selectDifficultPositionMain;

            var rightSpeed = isGoToEnd ? speedOpenDifficult : speedCloseDifficult;

            newGameFetchButton.image.sprite = isGoToEnd ? newGameSprite : normalNewGameFetchButtonSprite;

            newGameFetchButton.LockChangeSprite = true;
            
            if (!isGoToEnd)
                difficultWindow.SetActive(false);

            while (!IsOnPlace)
            {
                MainWindowPosition =
                    Vector2.MoveTowards(MainWindowPosition, TargetPosition,
                        rightSpeed * Time.deltaTime);

                hoverLight.gameObject.SetActive(false);

                yield return new WaitForEndOfFrame();
            }

            difficultWindow.SetActive(isGoToEnd);
            
            newGameFetchButton.LockChangeSprite = isGoToEnd;
        }
    }
}