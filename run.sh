#!/bin/bash

OPS_TYPE=$1
CMD_TYPE=$2

RED_TEXT=`tput setaf 1`
CYAN_TEXT=`tput setaf 6`
YELLOW_TEXT=`tput setaf 3`
GREEN_TEXT=`tput setaf 2`
RESET_TEXT=`tput sgr0`

function build_api {
  echo "${CYAN_TEXT}*** Building API ***${RESET_TEXT}"
  dotnet build Battleship.sln
}
function run_api_env {
  local environment="$1"
  local description="$2"
  export ASPNETCORE_ENVIRONMENT=$environment

  echo
  echo "${CYAN_TEXT}*** Running API --no-launch-profile  ${description} aka ASPNETCORE_ENVIRONMENT=${environment} ***${RESET_TEXT}"
  dotnet run --project Battleship.Api/Battleship.Api.csproj 
  
}

function run_unit_tests {
  echo
  echo ${GREEN_TEXT}*** Running Unit tests ***${RESET_TEXT}
  echo
  export GREP_OPTIONS='--color=always'
  export GREP_COLOR='1;35;40'
  dotnet test ./Battleship.Unit.Tests --verbosity normal
}

function run_integration_tests {
  echo
  echo
  echo ${GREEN_TEXT}*** Running Integration tests ***${RESET_TEXT}
  echo
  dotnet test ./Battleship.Integration.Tests
}


function print_usage {
  echo "Usage:"
  echo "${CYAN_TEXT}./run.sh build api"
  echo "${GREEN_TEXT}./run.sh run   api:development/|staging|production"
  echo "${YELLOW_TEXT}./run.sh test  unit"
  echo "${YELLOW_TEXT}./run.sh test  integration"
  echo "${RESET_TEXT}"
  exit
}

if [ "$OPS_TYPE" = "" ] || [ "$CMD_TYPE" = "" ]; then
  print_usage
fi

if [ $OPS_TYPE = "build" ]; then
  if [ $CMD_TYPE = "api" ]; then
    build_api
  else
    print_usage
  fi
elif [ $OPS_TYPE = "run" ]; then
  if [ $CMD_TYPE = "api:development" ]; then
    run_api_env "Development" "Dev Real Endpoints"
  elif [ $CMD_TYPE = "api:staging" ]; then
    run_api_env "Staging" "Real Endpoints"
  elif [ $CMD_TYPE = "api:production" ]; then
    run_api_env "Production" "Real Endpoints"
  else
    print_usage
  fi
elif [ $OPS_TYPE = "test" ]; then
  if [ $CMD_TYPE = "unit" ]; then
    run_unit_tests
  elif [ $CMD_TYPE = "integration" ]; then
    run_integration_tests
  else
    print_usage
  fi
else
  print_usage
fi
