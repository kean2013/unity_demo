local CodeGenEntry = {}

---@type HotfixCodeGenHandler
CodeGenEntry.HotfixCodeGenHandler = require(PluginPath .. "/HotfixCodeGenHandler")
---@type CodeGenConfig
CodeGenEntry.CodeGenConfig = require(PluginPath .. "/CodeGenConfig")

--- 点击发布工程时的回调
---@param handler CS.FairyEditor.PublishHandler 发布处理者
function onPublish(handler)
    CodeGenEntry.HotfixCodeGenHandler.Do(handler, CodeGenEntry.CodeGenConfig)
end

return CodeGenEntry

-- FYI: https://github.com/Tencent/xLua/blob/master/Assets/XLua/Doc/XLua_Tutorial_EN.md

-- local genCode = require(PluginPath..'/GenCode_CSharp')

-- function onPublish(handler)
--     if not handler.genCode then return end
--     handler.genCode = false --prevent default output

--     fprint('Handling gen code in plugin')
--     genCode(handler) --do it myself
-- end

-- function onDestroy()
-- -------do cleanup here-------
-- end
