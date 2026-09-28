param(
    [Parameter()]
    [String]$Server = "localhost",
    [Parameter()]
    [String]$Port = "5432",
    [Parameter()]
    [String]$Database = "",
    [Parameter(Mandatory = $true)]
    [String]$UserId,
    [Parameter(Mandatory = $true)]
    [String]$Password,
    [Parameter()]
    [String]$Target
)


if ([string]::IsNullOrEmpty($Database)) {
    $Database = Read-Host "Please enter database name"
}

$targetFlag = ""
if ($Target) {
    $targetFlag = "-t$Target"
}

yuniql run --platform postgresql -c "Server=$($Server);Port=$($Port);Database=$($Database);User Id=$($UserId);Password=$($Password)" $targetFlag -a -p "."
