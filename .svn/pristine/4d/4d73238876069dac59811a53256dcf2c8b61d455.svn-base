function genCode(handler) 
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    local ns = 'fgui'
    
    fprint("export path:" .. exportCodePath)
    
    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end
    
    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, ns)
    
    handler:SetupCodeFolder(exportCodePath, "ts") --check if target folder exists, and delete old files
 
    local getMemberByName = settings.getMemberByName
 
    local classCnt = classes.Count
    local writer = CodeWriter.new({ blockFromNewLine=false, usingTabs = true  })
 
    --FUIPackageStart--
    writer:writeln('namespace ETModel')
    writer:startBlock()
    writer:writeln('public static partial class FUIPackage')
    writer:startBlock()
 
    writer:writeln('public const string %s = "%s";',codePkgName,codePkgName)
 
    -- 生成所有的
    local itemCount = handler.items.Count
    for i=0,itemCount-1 do
        --fprint(handler.items[i].path)
        local path = string.format('%s%s%s',codePkgName,handler.items[i].path,handler.items[i].name)
        --writer:writeln('public const string %s = "ui://%s";//%s',string.gsub(path,'/','_'),string.format('%s/%s',codePkgName,handler.items[i].name),string.format('ui://%s%s',handler.pkg.id,handler.items[i].id))
        writer:writeln('public const string %s = "ui://%s";',string.gsub(path,'/','_'),string.format('%s%s',handler.pkg.id,handler.items[i].id))
    end
 
    writer:endBlock()--class
    writer:endBlock()--namespace
    local binderPackageName = 'Package'..codePkgName
    writer:save(exportCodePath..'/'..binderPackageName..'.cs')
    --FUIPackageEnd--
    
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        local members = classInfo.members
        local references = classInfo.references
        writer:reset()
 
        --local refCount = references.Count
        writer:writeln('using FairyGUI;')
        writer:writeln('using HotFix.Tool;')
        writer:writeln()
        writer:writeln('namespace HotFix.UI')
        writer:writeln()
        writer:startBlock()
        writer:writeln()
        writer:writeln('[ObjectSystem]')
        writer:writeln('public class %s_Comp : BaseUIComp',classInfo.className)
        writer:startBlock()
        writer:writeln()
        writer:writeln('public override void Awake(%s self, GObject go)',classInfo.className)
        writer:startBlock()
        writer:writeln('self.Awake(go);')
        writer:endBlock()
        writer:endBlock()
        writer:writeln()
 
 
        writer:writeln('public partial class %s : %s', classInfo.className, 'FUI')
        writer:startBlock()
        writer:writeln('public const string UIPackageName = "%s";', handler.pkg.name)
        writer:writeln('public const string UIResName = "%s";', classInfo.resName)
        writer:writeln('public const string UIName = "%s.%s";', handler.pkg.name,classInfo.resName)
        writer:writeln('public GComponent self;', classInfo.resName)
        writer:writeln()
 
        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            local typeName = memberInfo.type
            -- 判断是不是我们自定义类型
            if string.find(typeName,'fgui')==nil then
                writer:writeln('public %s %s;', typeName,memberInfo.varName)
                fprint('krma1 public %s %s;'.. typeName..' '..memberInfo.varName)
            else
                writer:writeln('public %s %s;', string.gsub(memberInfo.type,'fgui.',''),memberInfo.varName)
                fprint('krma2 public %s %s;'.. string.gsub(memberInfo.type,'fgui.','')..' '..memberInfo.varName)
            end
        end
        --writer:writeln('public static string URL = "ui://%s%s";', handler.pkg.id, classInfo.classId)
        writer:writeln()
 
        writer:writeln('public void Awake(GObject go)')
        writer:startBlock()
        writer:writeln('if (go == null)')
        writer:startBlock()
        writer:writeln('return;')
        writer:endBlock()
        writer:writeln('GObject = go;')
        writer:writeln('Name = UIName;')
        writer:writeln('self = (GComponent)go;')
        writer:writeln('self.Add(this);')
        writer:writeln('var com = go.asCom;')
        writer:writeln('if (com != null)')
        writer:startBlock()
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            local typeName = memberInfo.type
            if memberInfo.group==0 then
                 -- 判断是不是我们自定义类型
                if getMemberByName then
                    if string.find(typeName,'fgui')==nil then
                        writer:writeln('this.%s = ComponentFactory.Create<%s, GObject>( com.GetChildAt(%s));', memberInfo.varName, typeName, memberInfo.name)
                    else
                        writer:writeln('this.%s = (%s)com.GetChild("%s");', memberInfo.varName, string.gsub(memberInfo.type,'fgui.',''), memberInfo.name)
                    end
                else
                    if string.find(typeName,'fgui')==nil then
                        writer:writeln('this.%s = ComponentFactory.Create<%s, GObject>( com.GetChildAt(%s));', memberInfo.varName, typeName, memberInfo.index)
                    else
                        writer:writeln('this.%s = (%s)com.GetChildAt(%s);', memberInfo.varName, string.gsub(memberInfo.type,'fgui.',''), memberInfo.index)
                    end
                end
            elseif memberInfo.group==1 then
                if getMemberByName then
                    writer:writeln('this.%s = com.GetController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('this.%s = com.GetControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('this.%s = com.GetTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('this.%s = com.GetTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end
        --writer:writeln('OnInitialization();')
        writer:endBlock()
        writer:endBlock()
        writer:writeln()
        --writer:writeln('partial void OnInitialization();')
        writer:writeln()
        writer:writeln('public override void Dispose()')
        writer:startBlock()
        writer:writeln('base.Dispose();')
        writer:writeln('self.Remove();')
        writer:writeln('self = null;')
 
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            local typeName = memberInfo.type
            if memberInfo.group==0 then
                 -- 判断是不是我们自定义类型
                 if string.find(typeName,'fgui')==nil then
                    writer:writeln('%s.Dispose();', memberInfo.varName)
                end
                    writer:writeln('%s = null;', memberInfo.varName)
            else
                if getMemberByName then
                    writer:writeln('%s = null;', memberInfo.varName)
                else
                    writer:writeln('%s = null;', memberInfo.varName)
                end
            end
        end
 
        writer:endBlock()
        writer:endBlock() --class
        writer:endBlock()
 
        writer:save(exportCodePath..'/'..classInfo.className..'.cs')
        
        fprint('save to ' .. exportCodePath..'/'..classInfo.className..'.cs')
    end
 
end

return genCode