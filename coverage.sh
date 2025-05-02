#!/bin/bash
set -e

dotnet test --configuration Release --verbosity normal --logger trx --collect:"XPlat Code Coverage" 
dotnet coverage merge -o Coverage/merged.cobertura.xml -f cobertura UnitTests/**/coverage.cobertura.xml
reportgenerator -reports:"Coverage/merged.cobertura.xml" -targetdir:"Coverage" -reporttypes:Html