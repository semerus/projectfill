using GiraffeStar;

namespace FillClient
{
    public class FillBootstrap
    {

        public FillBootstrap()
        {
            Init();
        }

        void Init()
        {
            GiraffeSystem.Init();

            InitModules();
        }

        void InitModules()
        {
            GiraffeSystem.Register(new FillStateMachine());
        }
    }
}




