md coverage

REM packages\OpenCover.4.6.166\tools\OpenCover.Console.exe -register:user -target:packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe -targetargs:"/fixture:SafeMapper.Tests.ReflectionUtilsTests SafeMapper.Tests\bin\Debug\SafeMapper.Tests.dll /noshadow /xml=coverage\TestResult.xml" -filter:"+[SafeMapper*]* -[SafeMapper.Tests*]*" -output:coverage\opencovertests.xml

packages\OpenCover.4.6.166\tools\OpenCover.Console.exe -register:user -target:packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe -targetargs:"SafeMapper.Tests\bin\Debug\SafeMapper.Tests.dll /noshadow /xml=coverage\TestResult.xml" -filter:"+[SafeMapper*]* -[SafeMapper.Tests*]*" -output:coverage\opencovertests.xml

packages\ReportGenerator.2.4.0.0\tools\ReportGenerator.exe -reports:"coverage\opencovertests.xml" -targetdir:"coverage"

pause