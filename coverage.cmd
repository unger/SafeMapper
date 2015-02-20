md coverage

REM packages\OpenCover.4.5.3522\OpenCover.Console.exe -register:user -target:packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe -targetargs:"/fixture:SafeMapper.Tests.SafeConvertTests SafeMapper.Tests\bin\Debug\SafeMapper.Tests.dll /noshadow /xml=coverage\TestResult.xml" -filter:"+[SafeMapper*]* -[SafeMapper.Tests*]*" -output:coverage\opencovertests.xml

packages\OpenCover.4.5.3522\OpenCover.Console.exe -register:user -target:packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe -targetargs:"SafeMapper.Tests\bin\Debug\SafeMapper.Tests.dll /noshadow /xml=coverage\TestResult.xml" -filter:"+[SafeMapper*]* -[SafeMapper.Tests*]*" -output:coverage\opencovertests.xml

packages\ReportGenerator.2.1.1.0\ReportGenerator.exe -reports:"coverage\opencovertests.xml" -targetdir:"coverage"

pause