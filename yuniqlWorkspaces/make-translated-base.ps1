param(
    [Parameter()]
    [string]$Language = "english",
    [Parameter()]
    [string]$OutputPath = "./tmp",
    [Parameter()]
    [string]$BasePath = './base/',
    [Parameter()]
    [string]$LanguagesPath = './base-languages'
)

# Functions

function GetLanguagePath {
    param(
        [Parameter(Mandatory = $True)]
        [string]$Language,
        [Parameter(Mandatory = $True)]
        [string]$LanguagesPath
    )
    $LanguagePath = Join-Path -Path $LanguagesPath -ChildPath $Language
    if (-Not (Test-Path -Path $LanguagePath)) {
        Write-Host "Language is not present in languages directory"
        exit 1
    }
    return $LanguagePath
}

function Copy-Scripts {
    Param
    (
        [Parameter()]
        [string]$SourcePath,
        [Parameter()]
        [string]$DestinationPath,
        [Parameter()]
        [string]$AppendToDestinationFolders,
        [Parameter()]
        [string]$IsBaseWorkspace = $False
    )

    if ($IsBaseWorkspace -eq $True) {
        Copy-Item -Path $SourcePath -Destination $DestinationPath -Recurse;
        Get-ChildItem -Path $DestinationPath -Directory -Filter "v*" | Rename-Item -NewName { $_.Name + $AppendToDestinationFolders }
    }
    else {
        Get-ChildItem -Path $SourcePath -Directory -Filter "v*" | ForEach-Object {
            $NewName = $_.Name + $AppendToDestinationFolders
            Copy-Item -Path $_.FullName -Destination (Join-Path -Path $DestinationPath -ChildPath $NewName) -Recurse
        }
    }
}

# Script begins

if (Test-Path -Path $OutputPath) {
    Write-Host "Directory ""$OutputPath"" is not empty"
    exit 1
}

Copy-Scripts -SourcePath $BasePath -DestinationPath $OutputPath -AppendToDestinationFolders "_020-base" -IsBaseWorkspace $True

$Language = $Language.ToLower()
if ($Language -ine "english") {
    $LanguagePath = GetLanguagePath -Language $Language -LanguagesPath $LanguagesPath
    Copy-Scripts -SourcePath $LanguagePath -DestinationPath $OutputPath -AppendToDestinationFolders "_040-language_$($Language)" -IsBaseWorkspace $False
}

Write-Host "Successfully created YUNIQL workspace in $OutputPath"
