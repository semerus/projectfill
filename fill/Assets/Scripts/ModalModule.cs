using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GiraffeStar;
using UnityEngine.UI;
using DG.Tweening;
using DG;

namespace FillClient.UI
{
    public class DisplayModalMessage : MessageCore
    {
        public string message;
    }

    public class ModalModule : Module
    {
        GameObject modalPrefab;

        GameObject messageModal;

        bool isInitialized;

        public override void OnRegister()
        {
            base.OnRegister();
            Init();
        }

        void Init()
        {
            if(isInitialized) { return; }

            modalPrefab = Resources.Load<GameObject>("Prefab/ModalCanvas");
            messageModal = Object.Instantiate<GameObject>(modalPrefab).FindChildByName("ModalMessage");
            GameObject.DontDestroyOnLoad(messageModal.transform.root.gameObject);
            messageModal.SetActive(false);

            isInitialized = true;
        }

        [Subscriber]
        void Subscribe(DisplayModalMessage msg)
        {
            DisplayMessage(msg.message);
        }

        void DisplayMessage(string message)
        {
            ResetMessage();
            var text = messageModal.GetComponentInChildren<Text>();
            text.text = message;
            DOVirtual.DelayedCall(2f, CloseMessage);
        }

        void CloseMessage()
        {
            var image = messageModal.GetComponentInChildren<Image>();
            var text = messageModal.GetComponentInChildren<Text>();
            image.DOFade(0f, 0.5f).OnComplete(() =>
            {
                messageModal.SetActive(false);
            });
            text.DOFade(0f, 0.5f);
        }

        void ResetMessage()
        {
            messageModal.SetActive(true);
            var image = messageModal.GetComponentInChildren<Image>();
            var text = messageModal.GetComponentInChildren<Text>();
            image.color = image.color.OverrideAlpha(1f);
            text.color = text.color.OverrideAlpha(1f);
        }
    }
}


