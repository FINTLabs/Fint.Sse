pipeline {
  agent {
    docker {
      label 'docker'
      image 'microsoft/dotnet'
    }
  }
  stages {
    stage('Build') {
      when {
        not {
          buildingTag()
        }
      }
      steps {
        sh 'git clean -fdx'
        sh 'dotnet msbuild /t:restore /p:RestoreSources="https://api.nuget.org/v3/index.json;https://api.bintray.com/nuget/fint/nuget" Fint.Sse.sln'
        sh "dotnet msbuild /t:build;pack /p:Configuration=Release /p:BuildNumber=${BUILD_NUMBER} Fint.Sse.sln"
        sh 'dotnet test Fint.Sse.Tests'
      }
    }
    stage('Deploy') {
      environment {
        BINTRAY = credentials('fint-bintray')
      }
      when {
          tag pattern: "v\\d+\\.\\d+\\.\\d+(-\\w+-\\d+)?", comparator: "REGEXP"
      }
      steps {
          script {
              VERSION = TAG_NAME[1..-1]
          }
          sh "echo Version is ${VERSION}"
          sh 'git clean -fdx'
          sh "dotnet msbuild /t:restore;pack /p:Configuration=Release /p:Version=${VERSION} /p:BuildNumber=${BUILD_NUMBER} /p:RestoreSources=\"https://api.nuget.org/v3/index.json;https://api.bintray.com/nuget/fint/nuget\" Fint.Sse.sln"
          sh "dotnet nuget push Fint.Sse/bin/Release/Fint.Sse.*.nupkg -k ${BINTRAY} -s https://api.bintray.com/nuget/fint/nuget"
      }
    }
  }
}
