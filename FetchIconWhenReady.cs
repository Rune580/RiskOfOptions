using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RiskOfOptions
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

            Debug.Log(Thunderstore.defaultIcon);

            if (Thunderstore.defaultIcon)
            {
                Debug.Log(Thunderstore.defaultIcon.name);
            }

            StartCoroutine(SetTexture());
        }

        private void Update()
        {
            

        }

        private IEnumerator SetTexture()
        {
            yield return new WaitUntil(() => Thunderstore.doneFetching);

            var modIconInfo = Thunderstore.GetModIcon(modGuid);

            if (modIconInfo.Icon != string.Empty)
            {
                UnityWebRequest www = new UnityWebRequest($"file://{modIconInfo.Icon}");

                DownloadHandlerTexture downloadHandler = new DownloadHandlerTexture(true);

                www.downloadHandler = downloadHandler;

                www.SendWebRequest();

                yield return new WaitUntil(() => downloadHandler.isDone);

                _image.sprite = Sprite.Create(downloadHandler.texture, new Rect(0f, 0f, downloadHandler.texture.width, downloadHandler.texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
