/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

/** ============================================================================== **/
/** ============================Auto Generate Class=============================== **/
/** ============================================================================== **/

using FairyGUI;
using HotFix.Tool;
using System.Reflection;

namespace HotFix.UI
{
    public partial class LoginUIComp
    {   
            
    	public Controller m_State;
    	public GButton m_ServerSelectBtn;
    	public GButton m_GameStart;
    	public WL_LoginCP m_LoginCP;
    	public WL_Title m_Title;
    	public WL_CreateUser m_CreateUser;
    	public Transition m_GS_OP;
    	public Transition m_GS_CL;
    	public Transition m_LG_OP;
    	public Transition m_LG_CL;

        public void Init(GObject go)
        {
            if(go == null)
            {
                return;
            }
            
            var m_Comp = go.asCom;
                
            if(m_Comp != null)
            {   
                
    			m_State = m_Comp.GetController("State");
    			m_ServerSelectBtn = (GButton)m_Comp.GetChild("ServerSelectBtn");
    			m_GameStart = (GButton)m_Comp.GetChild("GameStart");
    			m_LoginCP = new WL_LoginCP(m_Comp.GetChild("LoginCP"));
    			m_Title = new WL_Title(m_Comp.GetChild("Title"));
    			m_CreateUser = new WL_CreateUser(m_Comp.GetChild("CreateUser"));
    			m_GS_OP = m_Comp.GetTransition("GS_OP");
    			m_GS_CL = m_Comp.GetTransition("GS_CL");
    			m_LG_OP = m_Comp.GetTransition("LG_OP");
    			m_LG_CL = m_Comp.GetTransition("LG_CL");
    		}
    	}

        public class WL_Title
        {   
                    
    		public GImage m_titleB;
    		public GImage m_titleW;

            public WL_Title(GObject go)
            {
                if(go == null)
                {
                    return;
                }
                
                var m_Comp = go.asCom;
                    
                if(m_Comp != null)
                {   
                    
    				m_titleB = (GImage)m_Comp.GetChild("titleB");
    				m_titleW = (GImage)m_Comp.GetChild("titleW");
    			}
    		}
    	}

        public class WL_LoginCP
        {   
                    
    		public GImage m_BG;
    		public GButton m_Close;
    		public GImage m_InputBG1;
    		public GTextInput m_username;
    		public GImage m_InputBG2;
    		public GTextInput m_password;
    		public GButton m_LoginBtn;

            public WL_LoginCP(GObject go)
            {
                if(go == null)
                {
                    return;
                }
                
                var m_Comp = go.asCom;
                    
                if(m_Comp != null)
                {   
                    
    				m_BG = m_Comp.GetChild("BG") as GImage;
    				m_Close = (GButton)m_Comp.GetChild("Close");
    				m_InputBG1 = (GImage)m_Comp.GetChild("InputBG1");
    				m_username = (GTextInput)m_Comp.GetChild("username");
    				m_InputBG2 = (GImage)m_Comp.GetChild("InputBG2");
    				m_password = (GTextInput)m_Comp.GetChild("password");
    				m_LoginBtn = (GButton)m_Comp.GetChild("LoginBtn");
                }
    		}
    	}

        public class WL_CreateUser
        {   
                    
    		public GImage m_BG;
    		public GButton m_Close;
    		public GImage m_InputBG1;
    		public GTextInput m_charName;
    		public GButton m_CreateBtn;

            public WL_CreateUser(GObject go)
            {
                if(go == null)
                {
                    return;
                }
                
                var m_Comp = go.asCom;
                    
                if(m_Comp != null)
                {   
                    
    				m_BG = (GImage)m_Comp.GetChild("BG");
    				m_Close = (GButton)m_Comp.GetChild("Close");
    				m_InputBG1 = (GImage)m_Comp.GetChild("InputBG1");
    				m_charName = (GTextInput)m_Comp.GetChild("charName");
    				m_CreateBtn = (GButton)m_Comp.GetChild("CreateBtn");
    			}
    		}
    	}

    }
}