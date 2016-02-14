$version = [System.Reflection.Assembly]::LoadFile("..\bin\Build\Gappalytics.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content Gappalytics.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File Gappalytics.compiled.nuspec
& nuget.exe pack Gappalytics.compiled.nuspec
