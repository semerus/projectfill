﻿using GiraffeStar;
using DG.Tweening;
using FillClient.UI;

namespace FillClient
{
    public class FillBootstrap
    {
        static bool isInitialized;

        public FillBootstrap()
        {
            if(isInitialized) { return; }

            Init();

            isInitialized = true;
        }

        void Init()
        {
            Config.Init();
            GiraffeSystem.Init();
            InitPlugins();
            InitModules();
        }

        void InitPlugins()
        {
            DOTween.Init().SetCapacity(200, 10);
        }

        void InitModules()
        {
            GiraffeSystem.Register(new FillStateMachine());
            GiraffeSystem.Register(new ModalModule());
        }
    }
}




