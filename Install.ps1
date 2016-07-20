param($rootPath, $toolsPath, $package, $project)

"Installing Specflow.NCrunch to project [{0}]" -f $project.FullName | Write-Host

#Write-Host $rootPath 
#Write-Host $toolsPath
#Write-Host $package
#Write-Host $project

#########################
# Start of script here
#########################

$projFile = $project.FullName
Write-Host "The project being installed into is $projFile"

# Make sure that the project file exists
if(!(Test-Path $projFile)){
    throw ("Project file not found at [{0}]" -f $projFile)
}

$projectDirectory = [System.IO.Path]::GetDirectoryName($project.FullName)
Push-Location $projectDirectory
$packageDllPath = "$rootPath\tools\"
$pluginPath = Resolve-Path -Relative $packageDllPath
Pop-Location


$xml = New-Object xml

# find the Web.config file
$config = $project.ProjectItems | where {$_.Name -eq "App.config"}

if ($config -eq $null)
{
	throw "There must be an app.config into which we can add the specflow plugin configuration"
}

# find its path on the file system
$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

# load Web.config as XML
$xml.Load($localPath.Value)

$xml.OuterXml |write-host

"Getting the specflow node" | write-host
# select the node
$specflowNode = $xml.SelectSingleNode("configuration/specFlow")

$specflowNode.OuterXml |Write-host

"Getting the plugins node" | write-host
$pluginsNode = $specflowNode.SelectSingleNode("plugins")
if ($pluginsNode -eq $null)
{
		"Theer was no plugins node, creating it" | write-host
	$pluginsNode = $xml.CreateElement("plugins") 
	$specflowNode.AppendChild($pluginsNode);
}
"Getting the ncrunch generator node" | write-host

$pluginNode = $pluginsNode.SelectSingleNode("add[@name='NCrunch.Genarator']")
if ($pluginNode -eq $null)
{
	"theer was no ncrunch generator node, creating one" | write-host
	$pluginNode = $xml.CreateElement("add")
	$pluginNode.SetAttribute("name","NCrunch.Generator")
	$pluginNode.SetAttribute("type","Generator")
	$pluginsNode.AppendChild($pluginNode)
}

"setting the path" | write-host
$pluginNode.SetAttribute("path",$pluginPath)


# save the Web.config file
$xml.Save($localPath.Value)


"    Specflow.NCrunch has been installed into project [{0}]" -f $project.FullName| Write-Host -ForegroundColor DarkGreen
