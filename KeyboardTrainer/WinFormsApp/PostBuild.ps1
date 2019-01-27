"Postbuild script starting... $(Get-Date -Format "%H:%m:%s")"
$ConfigurationName = "$(Split-Path -Leaf $(Get-Location))"
$OutputDir = "$(Get-Location)"

New-Item -ItemType Directory "Config" -ErrorAction SilentlyContinue | Out-Null

Push-Location "..\..\Config"
Foreach ($sourceFile in Get-ChildItem -Filter "*.ttxml" | Foreach {$_.FullName})
{
    $tempFile   = [System.IO.Path]::GetTempFileName()
    $targetFile = "$OutputDir\Config\" + [System.IO.Path]::GetFileNameWithoutExtension($sourceFile) + ".xml"

    & "${env:CommonProgramFiles(x86)}\Microsoft Shared\TextTemplating\14.0\TextTransform.exe" `
        -a !!ConfigName!$ConfigurationName $sourceFile -out $tempFile

    Get-Content $tempFile | Where-Object {$_ -notlike "*<!---->"} | Out-File $targetFile -Encoding "UTF8"
    Remove-Item $tempFile
}
Pop-Location

"Postbuild script finished. $(Get-Date -Format "%H:%m:%s")"
