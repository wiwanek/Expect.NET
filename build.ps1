properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\\" 
  $sln_file = "$base_dir\Expect.NET.sln" 
  $version = "1.0.2"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
} 

task default -depends Test

task Clean { 
  remove-item -force -recurse $buildartifacts_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
    new-item $release_dir -itemType directory 
    new-item $buildartifacts_dir -itemType directory 
} 

task Compile -depends Init { 
  Framework '4.5'
  msbuild /version
  echo " $sln_file"
  #msbuild $sln_file
  msbuild /p:OutDir=$buildartifacts_dir $sln_file
} 

task Test -depends Compile {
  Framework '4.5'
  $old = pwd
  cd $build_dir
    exec { & MSTest /testcontainer:Expect.Test.dll } 
  cd $old        
}
