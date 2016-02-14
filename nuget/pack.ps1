$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\bin\Build\Gappalytics.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\nuget\Gappalytics.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\nuget\Gappalytics.compiled.nuspec 
& $root\nuget\nuget.exe pack $root\nuget\Gappalytics.compiled.nuspec
