using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RiskOfOptions.OptionComponents
{
    public class FetchIconWhenReady : MonoBehaviour
    {
        public string modGuid;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        private void Start()
        {
            _image.sprite = Thunderstore.defaultIcon;

            StartCoroutine(SetTexture());
        }

        private void Update()
        {
            

        }

        private IEnumerator SetTexture()
        {
            yield return new WaitUntil(() => Thunderstore.doneFetching);

            var modIconInfo = Thunderstore.GetModIcon(modGuid);

            //Debug.Log($"modGuid: {modIconInfo.modGuid}, IconPath: {modIconInfo.IconPath}, Icon {modIconInfo.Icon}");

            if (!modIconInfo.Icon && modIconInfo.IconPath != string.Empty)
            {
                UnityWebRequest www = new UnityWebRequest($"file://{modIconInfo.IconPath}");

                DownloadHandlerTexture downloadHandler = new DownloadHandlerTexture(true);

                www.downloadHandler = downloadHandler;

                www.SendWebRequest();

                yield return new WaitUntil(() => downloadHandler.isDone);

                _image.sprite = Sprite.Create(downloadHandler.texture, new Rect(0f, 0f, downloadHandler.texture.width, downloadHandler.texture.height), new Vector2(0.5f, 0.5f));
            }
            else if (modIconInfo.Icon)
            {
                _image.sprite = modIconInfo.Icon;
            }
        }
    }
}
