param (
	[Parameter(Mandatory = $true)][string] $Version
)

$SourceDirectory = "$pwd\Source"
$PublishDirectory = "$SourceDirectory\Toffee.ConsoleClient\bin\Release\netcoreapp2.0\win10-x64\publish"
$ReleasesDirectory = "$pwd\Releases"
$ZipFileName = "Toffee-$Version.zip"
$ZipFilePath = "$ReleasesDirectory\$ZipFileName"

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

	$LinesCopy = New-Object System.Collections.ArrayList
	foreach ($Line in $FileContent)
	{
		if ($Line -like "*<Version>*")
		{
			$LinesCopy.Add("<Version>$Version</Version>")
		}
		elseif ($Line -like "*<AssemblyVersion>*")
		{
			$LinesCopy.Add("<AssemblyVersion>$Version</AssemblyVersion>")
		}
		elseif ($Line -like "*<FileVersion>*")
		{
			$LinesCopy.Add("<FileVersion>$Version</FileVersion>")
		}
		else
		{
			$LinesCopy.Add($Line)
		}
	}
	Write-Host $LinesCopy
	[System.IO.File]::WriteAllLines($ProjectFile, $LinesCopy)
}

#dotnet publish .\Source\Toffee.sln -c Release -f netcoreapp2.0 -r win10-x64 --self-contained

#Write-Host "Built and published solution to $PublishDirectory" -ForegroundColor Green

#Add-Type -AssemblyName "System.IO.Compression.Filesystem"
#[System.IO.Compression.ZipFile]::CreateFromDirectory($PublishDirectory, $ZipFilePath)

#Write-Host "Created $ZipFilePath" -ForegroundColor Green
