"Postbuild script starting... $(Get-Date -Format "%H:%m:%s")"
$ConfigurationName = "$(Split-Path -Leaf $(Get-Location))"

New-Item -Force -ItemType Directory -Name Config | Out-Null
Copy-Item ..\..\..\WinFormsApp\bin\$ConfigurationName\Config\*.xml Config

"Postbuild script finished. $(Get-Date -Format "%H:%m:%s")"
