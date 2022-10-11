using FairyGUI;
using HotFix.Tool;

namespace HotFix.UI
{

    public partial class LoginUIComp : BaseUIComp
    {
        GComponent m_Comp;

        public Controller Ctrl_Root;

        public LoginUIComp(GComponent c)
        {
            m_Comp = c;
            InitComp();
            Init(m_Comp);
        }

        public void InitComp()
        {

            Ctrl_Root = m_Comp.GetController("State");

        }


    }
}
