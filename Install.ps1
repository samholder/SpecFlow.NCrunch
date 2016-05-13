param($rootPath, $toolsPath, $package, $project)

"Installing Specflow.NCrunch to project [{0}]" -f $project.FullName | Write-Host

#Only for debugging
if(!$project){
    $project = (Get-Item C:\Temp\_NET\SlowCheetahMSBuild\SampleProject\SampleProject.csproj)

    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.Build")
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.Build.Engine")
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.Build.Framework")
}

$scLabel = "Specflow.NCrunch"

# When this package is installed we need to add a property
# to the current project, SlowCheetahTargets, which points to the
# .targets file in the packages folder

function RemoveExistingSlowCheetahPropertyGroups($projectRootElement){
    # if there are any PropertyGroups with a label of "SlowCheetah" they will be removed here
    $pgsToRemove = @()
    foreach($pg in $projectRootElement.PropertyGroups){
        if($pg.Label -and [string]::Compare($scLabel,$pg.Label,$true) -eq 0) {
            # remove this property group
            $pgsToRemove += $pg
        }
    }

    foreach($pg in $pgsToRemove){
        $pg.Parent.RemoveChild($pg)
    }
}

# TODO: Revisit this later, it was causing some exceptions
function CheckoutProjFileIfUnderScc(){
    # http://daltskin.blogspot.com/2012/05/nuget-powershell-and-tfs.html
    $sourceControl = Get-Interface $project.DTE.SourceControl ([EnvDTE80.SourceControl2])
    if($sourceControl.IsItemUnderSCC($project.FullName) -and $sourceControl.IsItemCheckedOut($project.FullName)){
        $sourceControl.CheckOutItem($project.FullName)
    }
}

function EnsureProjectFileIsWriteable(){
    $projItem = Get-ChildItem $project.FullName
    if($projItem.IsReadOnly) {
        "The project file is read-only. Please checkout the project file and re-install this package" | Write-Host -ForegroundColor Red
        throw;
    }
}

function ComputeRelativePathToTargetsFile(){
    param($startPath,$targetPath)
    
    # we need to compute the relative path
    $startLocation = Get-Location

    Set-Location $startPath.Directory | Out-Null
    $relativePath = Resolve-Path -Relative $targetPath.FullName

    # reset the location
    Set-Location $startLocation | Out-Null

    return $relativePath
}

function GetSolutionDirFromProj{
    param($msbuildProject)

    if(!$msbuildProject){
        throw "msbuildProject is null"
    }

    $result = $null
    $solutionElement = $null
    foreach($pg in $msbuildProject.PropertyGroups){
        foreach($prop in $pg.Properties){
            if([string]::Compare("SolutionDir",$prop.Name,$true) -eq 0){
                $solutionElement = $prop
                break
            }
        }
    }

    if($solutionElement){
        $result = $solutionElement.Value
    }

    return $result
}

# we need to update the packageRestore.proj file to have the correct value for SolutionDir
function UpdatePackageRestoreSolutionDir (){
    param($pkgRestorePath, $solDirValue)
    if(!(Test-Path $pkgRestorePath)){
        throw ("pkgRestore file not found at {0}" -f $pkgRestorePath)
    }

    $solDirElement = $null
    $root = [Microsoft.Build.Construction.ProjectRootElement]::Open($pkgRestorePath)
    foreach($pg in $root.PropertyGroups){
        foreach($prop in $pg.Properties){
            if([string]::Compare("SlowCheetahSolutionDir",$prop.Label,$true) -eq 0){
                $solDirElement = $prop
                break
            }
        }
    }
    
    if($solDirElement){
        $solDirElement.Value = $solDirValue

        $root.Save()

    }
}

function AddImportElementIfNotExists(){
    param($projectRootElement)

    $foundImport = $false
    $importsToRemove = @()
    foreach($import in $projectRootElement.Imports){
        $importStr = $import.Project
        if(!$importStr){
            $importStr = ""
        }

        if([string]::Compare('$(SlowCheetahTargets)',$importStr.Trim(),$true) -eq 0){
            if(!$foundImport){
               # if it doesn't have a label then add one
                if([string]::IsNullOrWhiteSpace($import.Label)){
                    $import.Label = $scLabel
                }
                $import.Condition="Exists('`$(SlowCheetahTargets)')"

                $foundImport = $true
            }
            else{
                # if we already found an import, this must be a duplicate remove it
                $importsToRemove+=$import
            }
        }
    }
    
    foreach($import in $importsToRemove){        
        # $projectRootElement.Imports.Remove($import)
        # you have to use Microsoft.Build.Evaluation.ProjectCollection to remove, so disabling should be good enough
        $import.Condition='false'
    }

    if(!$foundImport){
        # the import is not in the project, add it
        # <Import Project="$(SlowCheetahTargets)" Condition="Exists('$(SlowCheetahTargets)')" Label="SlowCheetah" />
        $importToAdd = $projectRootElement.AddImport('$(SlowCheetahTargets)');
        $importToAdd.Condition = "Exists('`$(SlowCheetahTargets)')"
        $importToAdd.Label = $scLabel 
    }        
}



#########################
# Start of script here
#########################

$projFile = $project.FullName
Write-Host "The project being installed into is $projFile"

# Make sure that the project file exists
if(!(Test-Path $projFile)){
    throw ("Project file not found at [{0}]" -f $projFile)
}

# use MSBuild to load the project and add the property
#We want to add the plugin dll to the project as a none building element

#  <ItemGroup>
#    <None Include="Specflow.NCrunch.SpecflowPlugin.dll" />  
#  </ItemGroup>



# EnsureProjectFileIsWriteable
# Before modifying the project save everything so that nothing is lost
$DTE.ExecuteCommand("File.SaveAll")
CheckoutProjFileIfUnderScc
EnsureProjectFileIsWriteable

$projectMSBuild = [Microsoft.Build.Construction.ProjectRootElement]::Open($projFile)
$itemGroup = $projectMSBuild.AddItemGroup()
$itemGroup.AddItem("None","NCrunch.Generator.SpecflowPlugin.dll")

$projectMSBuild.Save()







"    Specflow.NCrunch has been installed into project [{0}]" -f $project.FullName| Write-Host -ForegroundColor DarkGreen
