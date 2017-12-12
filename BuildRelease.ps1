param (
	[Parameter(Mandatory = $true)][string] $Version,
	[Parameter(Mandatory = $true)][string] $GitBranch,
	[Parameter(Mandatory = $true)][string] $GitTagMessage
)

if (git status --porcelain | Where {$_ -match '^\sM'})
{
	Write-Host "Git has changes. Commit them first and run this script again" -ForegroundColor Red
	return
}
else
{
	Write-Host "Git is clean" -ForegroundColor Green
}

$SourceDirectory = "$pwd\Source"
$PublishDirectory = "$SourceDirectory\Toffee.ConsoleClient\bin\Release\netcoreapp2.0\win10-x64\publish"
$ReleasesDirectory = "$pwd\Releases"
$ZipFileName = "Toffee-$Version.zip"
$ZipFilePath = "$ReleasesDirectory\$ZipFileName"
$Indent = "    "

if (Test-Path $PublishDirectory) 
{
	Remove-Item $PublishDirectory -Force -Recurse
}

if (!(Test-Path $ReleasesDirectory))
{
	New-Item -Path $ReleasesDirectory -ItemType Directory
}

$ProjectFiles = [System.IO.Directory]::GetFiles($SourceDirectory, "*.csproj", [System.IO.SearchOption]::AllDirectories) | where { $_ -notlike "*Tests*" }

foreach ($ProjectFile in $ProjectFiles) 
{
	$FileContent = [System.IO.File]::ReadAllLines($ProjectFile)

	$LinesCopy = New-Object 'System.Collections.Generic.List[String]'
	foreach ($Line in $FileContent)
	{
		if ($Line -like "*<Version>*")
		{
			$LinesCopy.Add("$Indent<Version>$Version</Version>")
		}
		elseif ($Line -like "*<AssemblyVersion>*")
		{
			$LinesCopy.Add("$Indent<AssemblyVersion>$Version</AssemblyVersion>")
		}
		elseif ($Line -like "*<FileVersion>*")
		{
			$LinesCopy.Add("$Indent<FileVersion>$Version</FileVersion>")
		}
		else
		{
			$LinesCopy.Add($Line)
		}
	}

	[System.IO.File]::WriteAllLines($ProjectFile, $LinesCopy)

	Write-Host "Bumped version of $ProjectFile to $Version" -ForegroundColor Green
}

dotnet publish .\Source\Toffee.sln -c Release -f netcoreapp2.0 -r win10-x64 --self-contained

Write-Host "Built and published solution to $PublishDirectory" -ForegroundColor Green

Add-Type -AssemblyName "System.IO.Compression.Filesystem"
[System.IO.Compression.ZipFile]::CreateFromDirectory($PublishDirectory, $ZipFilePath)

Write-Host "Created $ZipFilePath" -ForegroundColor Green

git add --a
git commit -m "Bumped version of csprojs to v$Version"
git tag -a "v$Version" -m "$GitTagMessage"
git push origin $GitBranch --follow-tags
