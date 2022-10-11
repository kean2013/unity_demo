---@class HotfixCodeGenHandler 热更层代码生成器
local HotfixCodeGenHandler = {}

--- 执行生成热更层代码
---@param handler CS.FairyEditor.PublishHandler
---@param codeGenConfig CodeGenConfig
function HotfixCodeGenHandler.Do(handler, codeGenConfig)
    fprint("开始生成Hotfix代码")

    local codePkgName = handler:ToFilename(handler.pkg.name) --convert chinese to pinyin, remove special chars etc.

    --- 从FGUI编辑器中读取配置
    ---@type CS.FairyEditor.GlobalPublishSettings.CodeGenerationConfig
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local getMemberByName = settings.getMemberByName
    local getMemberByNameSub = settings.getMemberByName

    --- 从自定义配置中读取路径和命名空间
    local exportCodePath = settings.codePath .. "/" .. codePkgName
    fprint(exportCodePath)
    fprint(settings.codePath)
    local namespaceName = codeGenConfig.HotfixNameSpace

    --- 初始化自定义组件名前缀
    local classNamePrefix = ""
    --settings.classNamePrefix
    --- 初始化自定义成员变量名前缀
    local memberVarNamePrefix = ""
    --settings.memberNamePrefix

    --- 所有将要导出的类（当前包的所有设置为导出的组件，以及当前包所有被引用的组件）
    ---@type CS.FairyEditor.PublishHandler.ClassInfo[]
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname)
    handler:SetupCodeFolder(exportCodePath, "cs") --check if target folder exists, and delete old files

    fprint("找到" .. classes.Count .. "个classes的数量")

    local classCnt = classes.Count
    local writer = CodeWriter.new()

    local mainPanelIndex = nil
    for i = 0, classCnt - 1 do
        local className = classes[i].className
        fprint("class name: " .. className)
        if (className == classNamePrefix .. settings.classNamePrefix .. "Root") then
            mainPanelIndex = i
            break
        end
    end

    --for i = 0, classCnt - 1 do
    -- for i = mainPanelIndex, mainPanelIndex do
    local classInfo = classes[mainPanelIndex]
    if classInfo == nil then
        classInfo = {}
        classInfo.members = {}
        classInfo.members.Count = 0
        fprint("Warning! " .. codePkgName .. "包里没有 Root 组件，生成了默认入口")
    end
    local members = classInfo.members
    writer:reset()

    writer:writeln("/** ============================================================================== **/")
    writer:writeln("/** ============================Auto Generate Class=============================== **/")
    writer:writeln("/** ============================================================================== **/")

    writer:writeln()
    writer:writeln("using FairyGUI;")
    writer:writeln("using HotFix.Tool;")
    -- writer:writeln("using FUIFW;")
    writer:writeln()
    writer:writeln("namespace %s", namespaceName)
    writer:startBlock()

    writer:writeln([[public partial class %s
    {   
            ]], codePkgName .. "Comp")

    local memberCnt = members.Count
    fprint("找到了" .. memberCnt .. " 个成员变量")

    -- 是否为自定义类型组件标记数组
    local customComponentFlagsArray = {}
    -- 是否为跨包组件标记数组
    local crossPackageFlagsArray = {}

    for j = 0, memberCnt - 1 do
        local memberInfo = members[j]
        customComponentFlagsArray[j] = false
        crossPackageFlagsArray[j] = false

        -- 判断是不是我们自定义类型组件
        local typeName = memberInfo.type
        for k = 0, classCnt - 1 do
            if typeName == classes[k].className then
                typeName = classNamePrefix .. classes[k].className
                customComponentFlagsArray[j] = true
                break
            end
        end

        -- -- 判断是不是跨包类型组件
        -- if memberInfo.res ~= nil then
        --     --- 组装自定义组件前缀
        --     typeName = classNamePrefix .. memberInfo.res.name
        --     crossPackageFlagsArray[j] = true
        -- end

        --- 组装自定义成员前缀
        writer:writeln("\tpublic %s %s;", typeName, memberVarNamePrefix .. memberInfo.varName)
    end

    writer:writeln()
    writer:writeln(
        [[
    public void Init(GObject go)
        {
            if (go == null)
            {
                return;
            }
            
            var m_Comp = go.asCom;
                
            if (m_Comp != null)
            {   
                ]]
    )
    
    local function asPostfix(className)
        local asStr = ""
        if className == "GComponent" then
            asStr = ".asCom"
        elseif className == "GImage" then
            asStr = ".asImage"
        elseif className == "GButton" then
            asStr = ".asButton"
        elseif className == "GLabel" then
            asStr = ".asLabel"
        elseif className == "GProgressBar" then
            asStr = ".asProgress"
        elseif className == "GSlider" then
            asStr = ".asSlider"
        elseif className == "GComboBox" then
            asStr = ".asComboBox"
        elseif className == "GTextField" then
            asStr = ".asTextField"
        elseif className == "GRichTextField" then
            asStr = ".asRichTextField"
        elseif className == "GTextInput" then
            asStr = ".asTextInput"
        elseif className == "GLoader" then
            asStr = ".asLoader"
        elseif className == "GLoader3D" then
            asStr = ".asLoader3D"
        elseif className == "GList" then
            asStr = ".asList"
        elseif className == "GGraph" then
            asStr = ".asGraph"
        elseif className == "GGroup" then
            asStr = ".asGroup"
        elseif className == "GMovieClip" then
            asStr = ".asMovieClip"
        elseif className == "GTree" then
            asStr = ".asTree"
        elseif className == "GTreeNode" then
            asStr = ".treeNode"
        end
        return asStr
    end

    for j = 0, memberCnt - 1 do
        local memberInfo = members[j]
        --- 组装自定义成员前缀
        local memberVarName = memberVarNamePrefix .. memberInfo.varName
        if memberInfo.group == 0 then
            if getMemberByName then
                if customComponentFlagsArray[j] then
                    --- 组装自定义组件前缀
                    writer:writeln(
                        '\t\t\t%s = new %s(m_Comp.GetChild("%s"));',
                        memberVarName,
                        classNamePrefix .. memberInfo.type,
                        memberInfo.name
                    )
                elseif crossPackageFlagsArray[j] then
                    --- 组装自定义组件前缀
                    writer:writeln(
                        '\t\t\t%s = new %s(m_Comp.GetChild("%s"));',
                        memberVarName,
                        classNamePrefix .. memberInfo.res.name,
                        memberInfo.name
                    )
                else
                    writer:writeln(
                        '\t\t\t%s = m_Comp.GetChild("%s")%s;',
                        memberVarName,
                        memberInfo.name,
                        asPostfix(memberInfo.type)
                    )
                end
            else
                if customComponentFlagsArray[j] then
                    --- 组装自定义组件前缀
                    writer:writeln(
                        "\t\t\t%s = new %s(m_Comp.GetChildAt(%s));",
                        memberVarName,
                        classNamePrefix .. memberInfo.type,
                        memberInfo.index
                    )
                elseif crossPackageFlagsArray[j] then
                    --- 组装自定义组件前缀
                    writer:writeln(
                        "\t\t\t%s = new %s(m_Comp.GetChildAt(%s));",
                        memberVarName,
                        classNamePrefix .. memberInfo.res.name,
                        memberInfo.index
                    )
                else
                    writer:writeln(
                        "\t\t\t%s = m_Comp.GetChildAt(%s)%s;",
                        memberVarName,
                        memberInfo.index,
                        asPostfix(memberInfo.type)
                    )
                end
            end
        elseif memberInfo.group == 1 then
            if getMemberByName then
                writer:writeln('\t\t\t%s = m_Comp.GetController("%s");', memberVarName, memberInfo.name)
            else
                writer:writeln("\t\t\t%s = m_Comp.GetControllerAt(%s);", memberVarName, memberInfo.index)
            end
        else
            if getMemberByName then
                writer:writeln('\t\t\t%s = m_Comp.GetTransition("%s");', memberVarName, memberInfo.name)
            else
                writer:writeln("\t\t\t%s = m_Comp.GetTransitionAt(%s);", memberVarName, memberInfo.index)
            end
        end
    end
    writer:writeln("\t\t}")

    writer:writeln("\t}")
    writer:writeln()

    local classCntSub = classes.Count
    fprint("找到了" .. classCntSub - 1 .. "个子对象")
    for m = 0, classCntSub - 1 do
        if (m ~= mainPanelIndex) then
            local classInfoSub = classes[m]
            local membersSub = classInfoSub.members
            local classNameSub = classNamePrefix .. classInfoSub.className
            fprint("krma." .. classNameSub)

            writer:writeln(
                [[
    public class %s
        {   
                    ]],
                classNameSub
                -- .. " : " .. classInfoSub.superClassName
            )

            local memberCntSub = membersSub.Count
            -- 是否为自定义类型组件标记数组
            local customComponentFlagsArraySub = {}
            -- 是否为跨包组件标记数组
            local crossPackageFlagsArraySub = {}

            for n = 0, memberCntSub - 1 do
                local memberInfoSub = membersSub[n]

                fprint("krma............" .. memberInfoSub.varName)
                customComponentFlagsArraySub[n] = false
                crossPackageFlagsArraySub[n] = false

                -- 判断是不是我们自定义类型组件
                local typeNameSub = memberInfoSub.type
                for k = 0, classCntSub - 1 do
                    if typeNameSub == classes[k].className then
                        typeNameSub = classNamePrefix .. classes[k].className
                        customComponentFlagsArraySub[n] = true
                        break
                    end
                end

                -- 判断是不是跨包类型组件
                -- if memberInfoSub.res ~= nil then
                --     --- 组装自定义组件前缀
                --     typeNameSub = classNamePrefix .. memberInfoSub.res.name
                --     crossPackageFlagsArraySub[n] = true
                -- end

                --- 组装自定义成员前缀
                writer:writeln("\t\tpublic %s %s;", typeNameSub, memberVarNamePrefix .. memberInfoSub.varName)
            end
            if classInfoSub.superClassName == "GComponent" then
                writer:writeln("\t\tpublic GComponent m_com;")
            elseif classInfoSub.superClassName == "GImage" then
                writer:writeln("\t\tpublic GImage m_img;")
            elseif classInfoSub.superClassName == "GButton" then
                writer:writeln("\t\tpublic GButton m_btn;")
            elseif classInfoSub.superClassName == "GLabel" then
                writer:writeln("\t\tpublic GLabel m_lbl;")
            elseif classInfoSub.superClassName == "GProgressBar" then
                writer:writeln("\t\tpublic GProgressBar m_proBar;")
            elseif classInfoSub.superClassName == "GSlider" then
                writer:writeln("\t\tpublic GSlider m_sld;")
            elseif classInfoSub.superClassName == "GComboBox" then
                writer:writeln("\t\tpublic GComboBox m_cmbBox;")
            elseif classInfoSub.superClassName == "GTextField" then
                writer:writeln("\t\tpublic GTextField m_txtFld;")
            elseif classInfoSub.superClassName == "GRichTextField" then
                writer:writeln("\t\tpublic GRichTextField m_richTxtFld;")
            elseif classInfoSub.superClassName == "GTextInput" then
                writer:writeln("\t\tpublic GTextInput m_txtIpt;")
            elseif classInfoSub.superClassName == "GLoader" then
                writer:writeln("\t\tpublic GLoader m_loader;")
            elseif classInfoSub.superClassName == "GLoader3D" then
                writer:writeln("\t\tpublic GLoader3D m_loader3d;")
            elseif classInfoSub.superClassName == "GList" then
                writer:writeln("\t\tpublic GList m_list;")
            elseif classInfoSub.superClassName == "GGraph" then
                writer:writeln("\t\tpublic GGraph m_graph;")
            elseif classInfoSub.superClassName == "GGroup" then
                writer:writeln("\t\tpublic GGroup m_group;")
            elseif classInfoSub.superClassName == "GMovieClip" then
                writer:writeln("\t\tpublic GMovieClip m_movClip;")
            elseif classInfoSub.superClassName == "GTree" then
                writer:writeln("\t\tpublic GTree m_tree;")
            elseif classInfoSub.superClassName == "GTreeNode" then
                writer:writeln("\t\tpublic GTreeNode m_treeNode;")
            end
            writer:writeln()
            writer:writeln(
                [[
        public %s(GObject go)
            {
                if (go == null)
                {
                    return;
                }
                
                var m_Comp = go.asCom;
                    
                if (m_Comp != null)
                {   
                    ]],
                classNameSub
            )

            for j = 0, memberCntSub - 1 do
                local memberInfoSub = membersSub[j]
                --- 组装自定义成员前缀
                local memberVarNameSub = memberVarNamePrefix .. memberInfoSub.varName
                if memberInfoSub.group == 0 then
                    if getMemberByNameSub then
                        if customComponentFlagsArraySub[j] then
                            --- 组装自定义组件前缀
                            writer:writeln(
                                '\t\t\t\t%s = new %s(m_Comp.GetChild("%s"));',
                                memberVarNameSub,
                                classNamePrefix .. memberInfoSub.type,
                                memberInfoSub.name
                            )
                        elseif crossPackageFlagsArraySub[j] then
                            --- 组装自定义组件前缀
                            writer:writeln(
                                '\t\t\t\t%s = new %s(m_Comp.GetChild("%s"));',
                                memberVarNameSub,
                                classNamePrefix .. memberInfoSub.res.name,
                                memberInfoSub.name
                            )
                        else
                            writer:writeln(
                                '\t\t\t\t%s = m_Comp.GetChild("%s")%s;',
                                memberVarNameSub,
                                memberInfoSub.name,
                                asPostfix(memberInfoSub.type)
                            )
                        end
                    else
                        if customComponentFlagsArraySub[j] then
                            --- 组装自定义组件前缀
                            writer:writeln(
                                "\t\t\t\t%s = new %s(m_Comp.GetChildAt(%s));",
                                memberVarNameSub,
                                classNamePrefix .. memberInfoSub.type,
                                memberInfoSub.index
                            )
                        elseif crossPackageFlagsArraySub[j] then
                            --- 组装自定义组件前缀
                            writer:writeln(
                                "\t\t\t\t%s = new %s(m_Comp.GetChildAt(%s));",
                                memberVarNameSub,
                                classNamePrefix .. memberInfoSub.res.name,
                                memberInfoSub.index
                            )
                        else
                            writer:writeln(
                                "\t\t\t\t%s = m_Comp.GetChildAt(%s)%s;",
                                memberVarNameSub,
                                memberInfoSub.index,
                                asPostfix(memberInfoSub.type)
                            )
                        end
                    end
                elseif memberInfoSub.group == 1 then
                    if getMemberByNameSub then
                        writer:writeln('\t\t\t\t%s = m_Comp.GetController("%s");', memberVarNameSub, memberInfoSub.name)
                    else
                        writer:writeln(
                            "\t\t\t\t%s = m_Comp.GetControllerAt(%s);",
                            memberVarNameSub,
                            memberInfoSub.index
                        )
                    end
                else
                    if getMemberByNameSub then
                        writer:writeln('\t\t\t\t%s = m_Comp.GetTransition("%s");', memberVarNameSub, memberInfoSub.name)
                    else
                        writer:writeln(
                            "\t\t\t\t%s = m_Comp.GetTransitionAt(%s);",
                            memberVarNameSub,
                            memberInfoSub.index
                        )
                    end
                end
            end

            if classInfoSub.superClassName == "GComponent" then
                writer:writeln("\t\t\t\tm_com = m_Comp;")
            elseif classInfoSub.superClassName == "GImage" then
                writer:writeln("\t\t\t\tm_img = m_Comp.asImage;")
            elseif classInfoSub.superClassName == "GButton" then
                writer:writeln("\t\t\t\tm_btn = m_Comp.asButton;")
            elseif classInfoSub.superClassName == "GLabel" then
                writer:writeln("\t\t\t\tm_lbl = m_Comp.asLabel;")
            elseif classInfoSub.superClassName == "GProgressBar" then
                writer:writeln("\t\t\t\tm_proBar = m_Comp.asProgress;")
            elseif classInfoSub.superClassName == "GSlider" then
                writer:writeln("\t\t\t\tm_sld = m_Comp.asSlider;")
            elseif classInfoSub.superClassName == "GComboBox" then
                writer:writeln("\t\t\t\tm_cmbBox = m_Comp.asComboBox;")
            elseif classInfoSub.superClassName == "GTextField" then
                writer:writeln("\t\t\t\tm_txtFld = m_Comp.asTextField;")
            elseif classInfoSub.superClassName == "GRichTextField" then
                writer:writeln("\t\t\t\tm_richTxtFld = m_Comp.asRichTextField;")
            elseif classInfoSub.superClassName == "GTextInput" then
                writer:writeln("\t\t\t\tm_txtIpt = m_Comp.asTextInput;")
            elseif classInfoSub.superClassName == "GLoader" then
                writer:writeln("\t\t\t\tm_loader = m_Comp.asLoader;")
            elseif classInfoSub.superClassName == "GLoader3D" then
                writer:writeln("\t\t\t\tm_loader3d = m_Comp.asLoader3D;")
            elseif classInfoSub.superClassName == "GList" then
                writer:writeln("\t\t\t\tm_list = m_Comp.asList;")
            elseif classInfoSub.superClassName == "GGraph" then
                writer:writeln("\t\t\t\tm_graph = m_Comp.asGraph;")
            elseif classInfoSub.superClassName == "GGroup" then
                writer:writeln("\t\t\t\tm_group = m_Comp.asGroup;")
            elseif classInfoSub.superClassName == "GMovieClip" then
                writer:writeln("\t\t\t\tm_movClip = m_Comp.asMovieClip;")
            elseif classInfoSub.superClassName == "GTree" then
                writer:writeln("\t\t\t\tm_tree = m_Comp.asTree;")
            elseif classInfoSub.superClassName == "GTreeNode" then
                writer:writeln("\t\t\t\tm_treeNode = m_Comp.treeNode;")
            end
            writer:writeln("\t\t\t}")

            writer:writeln("\t\t}")

            writer:writeln("\t}")
            writer:writeln()
        end
    end

    writer:writeln("}")
    writer:endBlock()

    path = exportCodePath .. "/" .. codePkgName .. "AutoComp.cs"
    writer:save(path)
    fprint("生成代码完毕：" .. path)
    -- end
end

return HotfixCodeGenHandler
