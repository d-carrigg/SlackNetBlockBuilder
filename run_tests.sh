#!/bin/bash
cd UnitTests
dotnet test --no-build --verbosity normal > test_results.txt 2>&1
echo "Tests completed. Results saved to UnitTests/test_results.txt" 