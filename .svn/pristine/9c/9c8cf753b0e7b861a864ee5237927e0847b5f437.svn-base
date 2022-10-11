using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FairyGUI.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlEvent : IHtmlObject
    {
        RichTextField _owner;
        HtmlElement _element;

        string _src;
        string _act;

        public HtmlEvent(string action)
        {
            _act = action;
        }

        public DisplayObject displayObject
        {
            get { return null; }
        }

        public HtmlElement element
        {
            get { return _element; }
        }

        public float width
        {
            get { return 0; }
        }

        public float height
        {
            get { return 0; }
        }

        public void Create(RichTextField owner, HtmlElement element)
        {
            _owner = owner;
            _element = element;

            _src = element.GetString("src");
        }

        public void SetArea(int startLine, float startCharX, int endLine, float endCharX)
        {
        }

        public void SetPosition(float x, float y)
        {
        }

        public void Add()
        {
           //  UnityEngine.Debug.Log("Dispatch: " + _act + "... " + _src);
            _owner.DispatchEvent("htmlEvent", new { action = _act, src = _src});
        }

        public void Remove()
        {

        }

        public void Release()
        {
            _owner = null;
            _element = null;
        }

        public void Dispose()
        {
        }
    }
}
