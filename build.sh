#!/usr/bin/env bash
set -euox pipefail

dotnet tool restore

dotnet cake --verbosity=diagnostic $@
