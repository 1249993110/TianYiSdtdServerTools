; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "天依七日杀服主工具"
#define MyAppVersion "4.0.0.0"
#define MyAppPublisher "冰咖啡"
#define MyAppURL "https://7daystodie.top"
#define MyAppExeName "SdtdServerTools.exe"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (若要生成新的 GUID，可在菜单中点击 "工具|生成 GUID"。)
AppId={{31AFC86C-1701-4847-8675-BCF2F8427C24}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=G:\VS2017 Projects\TianYiSdtdServerTools\打包工具\许可证.txt
; 移除以下行，以在管理安装模式下运行（为所有用户安装）。
PrivilegesRequired=lowest
OutputDir=G:\VS2017 Projects\TianYiSdtdServerTools\打包工具\publish
OutputBaseFilename=天依七日杀服主工具 Beta 4.0
SetupIconFile=G:\VS2017 Projects\TianYiSdtdServerTools\资源\image\installPackage.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "G:\VS2017 Projects\TianYiSdtdServerTools\Client\Views\bin\Release\SdtdServerTools.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "G:\VS2017 Projects\TianYiSdtdServerTools\Client\Views\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

