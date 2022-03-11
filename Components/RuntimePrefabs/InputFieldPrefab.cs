using RiskOfOptions.Components.Options;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiskOfOptions.Components.RuntimePrefabs
{
    public class InputFieldPrefab : IRuntimePrefab // This is kind of a mess right now
    {
        public GameObject InputField { get; private set; }
        
        public void Instantiate(GameObject settingsPanel)
        {
            Transform verticalLayout = settingsPanel.transform.Find("SafeArea").Find("SubPanelArea")
                .Find("SettingsSubPanel, Audio").Find("Scroll View").Find("Viewport").Find("VerticalLayout");
            
            InputField = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject);
            InputField.name = "Mod Option Prefab, InputField";
            InputField.SetActive(false);
            
            Object.DestroyImmediate(InputField.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(InputField.transform.Find("CarouselRect").gameObject);
            
            var controller = InputField.AddComponent<InputFieldController>();

            ColorBlock inputColors = InputField.GetComponent<HGButton>().colors;
            
            GameObject textPreview = new GameObject("Text Preview", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(ContentSizeFitter));
            
            textPreview.transform.SetParent(InputField.transform);
            
            var textPreviewHorizontalLayoutGroup = textPreview.GetComponent<HorizontalLayoutGroup>();
            
            textPreviewHorizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
            textPreviewHorizontalLayoutGroup.padding = new RectOffset(8, 8, 0, 0);
            textPreviewHorizontalLayoutGroup.spacing = 8;
            textPreviewHorizontalLayoutGroup.childForceExpandWidth = false;
            textPreviewHorizontalLayoutGroup.childForceExpandHeight = false;
            
            var textPreviewRectTransform = textPreview.GetComponent<RectTransform>();
            textPreviewRectTransform.anchorMin = new Vector2(0, 0);
            textPreviewRectTransform.anchorMax = new Vector2(1, 1);
            textPreviewRectTransform.pivot = new Vector2(1, 0.5f);
            textPreviewRectTransform.anchoredPosition = new Vector2(-6, 0);

            var textPreviewSizeFitter = textPreview.GetComponent<ContentSizeFitter>();
            textPreviewSizeFitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            textPreviewSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            
            var previewButton = Object.Instantiate(verticalLayout.Find("SettingsEntryButton, Bool (Audio Focus)").gameObject, textPreview.transform);
            previewButton.name = "Text Preview Button";
            
            Object.DestroyImmediate(previewButton.GetComponentInChildren<CarouselController>());
            Object.DestroyImmediate(previewButton.transform.Find("CarouselRect").gameObject);

            var previewLayoutElement = previewButton.GetComponent<LayoutElement>();
            previewLayoutElement.minWidth = 256;
            previewLayoutElement.minHeight = 48;
            previewLayoutElement.preferredWidth = 256;
            
            
            GameObject textCanvas = new GameObject("Text Overlay", typeof(RectTransform), typeof(RectMask2D), typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup));
            
            textCanvas.SetActive(false);
            textCanvas.transform.SetParent(InputField.transform);
            
            //var textCanvasImage = textCanvas.AddComponent<Image>(_inputFieldPrefab.GetComponent<Image>());
            
            //textCanvas.AddComponent<TranslucentImage>(Prefabs.MoPanelPrefab.transform.Find("Scroll View").Find("BlurPanel").gameObject.GetComponent<TranslucentImage>());
            
            //textCanvasImage.color = inputColors.normalColor;
            
            var inputFieldOverlay = textCanvas.AddComponent<RooInputField>();
            
            var textCanvasRectTransform = textCanvas.GetComponent<RectTransform>();
            
            textCanvasRectTransform.anchoredPosition = new Vector2(0, -50);
            textCanvasRectTransform.sizeDelta = new Vector2(525, 48);
            
            textCanvas.name = "Text Overlay";


            GameObject textArea = new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D), typeof(CanvasRenderer), typeof(GraphicRaycaster));
            
            textArea.transform.SetParent(textCanvas.transform);
            
            var textAreaRectTransform = textArea.GetComponent<RectTransform>();
            
            textAreaRectTransform.anchorMin = new Vector2(0, 0);
            textAreaRectTransform.anchorMax = new Vector2(1, 1);
            textAreaRectTransform.anchoredPosition = Vector2.zero;
            textAreaRectTransform.sizeDelta = Vector2.zero;
            
            var textAreaImage = textArea.AddComponent(InputField.GetComponent<Image>());
            
            textAreaImage.color = inputColors.normalColor;
            
            textArea.name = "Text Area";
            
            
            GameObject inputText = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer)); 
            
            inputText.transform.SetParent(textArea.transform);
            
            var inputTextMesh = inputText.AddComponent(InputField.transform.Find("ButtonText").GetComponent<HGTextMeshProUGUI>());
            inputTextMesh.fontSizeMin = 24;
            inputTextMesh.fontSizeMax = 24;
            inputTextMesh.enableWordWrapping = true;
            inputTextMesh.margin = new Vector4(12, 12, 12, 12);
            
            var inputTextRectTransform = inputText.GetComponent<RectTransform>();
            inputTextRectTransform.anchorMin = new Vector2(0.02f, 0);
            inputTextRectTransform.anchorMax = new Vector2(0.98f, 1);
            inputTextRectTransform.anchoredPosition = Vector2.zero; 
            inputTextRectTransform.sizeDelta = Vector2.zero;
            
            inputText.name = "Text";

            inputFieldOverlay.textViewport = textAreaRectTransform;
            inputFieldOverlay.textComponent = inputText.GetComponent<HGTextMeshProUGUI>();
            inputFieldOverlay.lineType = TMP_InputField.LineType.MultiLineNewline;
            inputFieldOverlay.colors = inputColors;

            controller.overlay = textCanvas;
            controller.inputField = inputFieldOverlay;
        }

        public void Destroy()
        {
            Object.DestroyImmediate(InputField);
        }
    }
}