Add-Type -Assembly "System.Io.Compression.FileSystem"

$unixEncoderSource = @"
    using System.Text;
    public class UnixEncoder : UTF8Encoding
        {
            public UnixEncoder() : base(true)
            {
            
            }

            public override byte[] GetBytes(string s)
            {
                s = s.Replace("\\", "/");
                return base.GetBytes(s);
            }
    }
"@

Add-Type -TypeDefinition $unixEncoderSource
$unixEncoder = New-Object UnixEncoder

function Copy-New-Item {
  $SourceFilePath = $args[0]
  $DestinationFilePath = $args[1]

  If (-not (Test-Path $DestinationFilePath)) {
    New-Item -ItemType File -Path $DestinationFilePath -Force
  } 
  Copy-Item -Path $SourceFilePath -Destination $DestinationFilePath
}

$currentPath = (Get-Item -Path ".\").FullName

Write-Host "Creating binary files"
Write-Host "Working directory: $currentPath"

Copy-New-Item ..\..\..\icons\diagram.ico dist\bin\diagram.ico
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.CodeGenerator.dll dist\bin\NClass.CodeGenerator.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.Core.dll dist\bin\NClass.Core.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.CSharp.dll dist\bin\NClass.CSharp.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.Dart.dll dist\bin\NClass.Dart.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.DiagramEditor.dll dist\bin\NClass.DiagramEditor.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.exe dist\bin\NClass.exe
Copy-New-Item ..\..\..\GUI\bin\Release\Octokit.dll dist\bin\Octokit.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.exe.config dist\bin\NClass.exe.config
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.Java.dll dist\bin\NClass.Java.dll
Copy-New-Item ..\..\..\GUI\bin\Release\NClass.Translations.dll dist\bin\Lang\NClass.Translations.dll
Copy-New-Item ..\..\..\GUI\bin\Release\de\NClass.Translations.resources.dll dist\bin\Lang\de\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\es\NClass.Translations.resources.dll dist\bin\Lang\es\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\hu\NClass.Translations.resources.dll dist\bin\Lang\hu\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\pt-BR\NClass.Translations.resources.dll dist\bin\Lang\pt-BR\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\ru\NClass.Translations.resources.dll dist\bin\Lang\ru\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\zh-CN\NClass.Translations.resources.dll dist\bin\Lang\zh-CN\NClass.Translations.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\AssemblyImport.dll dist\bin\Plugins\AssemblyImport.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\NReflect.dll dist\bin\Plugins\NReflect.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\PDFExport.dll dist\bin\Plugins\PDFExport.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\PdfSharp-gdi.dll dist\bin\Plugins\PdfSharp-gdi.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\PdfSharp.Charting-gdi.dll dist\bin\Plugins\PdfSharp.Charting-gdi.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\de\AssemblyImport.resources.dll dist\bin\Plugins\de\AssemblyImport.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\de\PDFExport.resources.dll dist\bin\Plugins\de\PDFExport.resources.dll
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\Templates\csproj.template dist\bin\Templates\csproj.template
Copy-New-Item ..\..\..\GUI\bin\Release\Plugins\Templates\sln.template dist\bin\Templates\sln.template
Copy-New-Item ..\..\..\..\doc\changelog.txt dist\doc\changelog.txt
Copy-New-Item ..\..\..\..\doc\commonmark.net.license.txt dist\doc\commonmark.net.license.txt
Copy-New-Item ..\..\..\..\doc\ocktokit.license.txt dist\doc\ocktokit.license.txt
Copy-New-Item ..\..\..\..\doc\pdfsharp.license.txt dist\doc\pdfsharp.license.txt
Copy-New-Item ..\..\..\..\doc\license.txt dist\doc\license.txt
Copy-New-Item ..\..\..\..\doc\readme.txt dist\doc\readme.txt
Copy-New-Item ..\..\..\..\examples\shapes.ncp dist\examples\shapes.ncp
Copy-New-Item "..\..\..\..\styles\Visual Studio Class Designer (ClearType).dst" "dist\styles\Visual Studio Class Designer (ClearType).dst"
Copy-New-Item "..\..\..\..\styles\Visual Studio Class Designer.dst" "dist\styles\Visual Studio Class Designer.dst"
Copy-New-Item ..\..\..\GUI\bin\Release\CommonMark.dll dist\bin\CommonMark.dll

$source = Join-Path $currentPath "dist"
$destinationFile = Join-Path $currentPath "nclass.zip"

if(Test-Path $destinationFile) {
    Remove-Item $destinationFile
}

[io.compression.zipfile]::CreateFromDirectory($source, $destinationFile, [io.compression.CompressionLevel]::Fastest, $false, $unixEncoder) 
Remove-Item $source -Recurse -Force

Write-Host "Done"
