#!/usr/bin/env bash
set -e

# Wait for Postgres to be available using TCP probe.
# Usage: ./wait-for-postgres.sh <host> <port> [timeout_seconds]

HOST=${1:-}
PORT=${2:-}
TIMEOUT=${3:-60}

# If positional args are empty or not provided, fall back to environment variables
# and finally to sane defaults. This avoids an empty HOST/PORT when Compose passes
# empty strings as arguments.
if [ -z "${HOST}" ]; then
  HOST=${POSTGRES_HOST:-}
fi
if [ -z "${PORT}" ]; then
  PORT=${POSTGRES_PORT:-5432}
fi

# Resolve a list of candidate addresses to try connecting to. This makes the
# script robust when the DB container isn't attached to the same Docker
# network (e.g., it's running on the host or created separately).
# Order: explicit HOST, service name 'db', POSTGRES_HOST env, docker gateway,
# host.docker.internal (useful on some platforms).
GATEWAY="$(ip route 2>/dev/null | awk '/default/ {print $3; exit}')"
HOST_DOCKER_INTERNAL="host.docker.internal"

echo "Waiting for Postgres (timeout: ${TIMEOUT}s). Candidates will be tried in order..."
echo "  initial args -> host: '${HOST:-<none>}' port: '${PORT}'"
start_ts=$(date +%s)

# Build candidate list
candidates=()
if [ -n "${HOST}" ]; then
  candidates+=("${HOST}")
fi
# common compose service name
candidates+=("db")
# explicit env override
if [ -n "${POSTGRES_HOST}" ]; then
  candidates+=("${POSTGRES_HOST}")
fi
if [ -n "${GATEWAY}" ]; then
  candidates+=("${GATEWAY}")
fi
# host.docker.internal may work on some systems
candidates+=("${HOST_DOCKER_INTERNAL}")

found=0
while true; do
  for target in "${candidates[@]}"; do
    [ -z "${target}" ] && continue
    # try to open TCP connection
    if (echo > /dev/tcp/${target}/${PORT}) >/dev/null 2>&1; then
      echo "Postgres is available at ${target}:${PORT}"
      found=1
      break
    else
      echo "Tried ${target}:${PORT} - no response"
    fi
  done

  if [ ${found} -eq 1 ]; then
    break
  fi

  now_ts=$(date +%s)
  elapsed=$((now_ts - start_ts))
  if [ ${elapsed} -ge ${TIMEOUT} ]; then
    echo "Timed out waiting for Postgres after ${TIMEOUT} seconds" >&2
    exit 1
  fi

  sleep 1
done

# Exec the original command (dotnet) passed as arguments to the script.
# We preserve the original positional parameters and run any command given
# after the host/port/timeout parameters. If no additional command is present
# we default to starting the app with dotnet.
ORIG_ARGS=("$@")
if [ ${#ORIG_ARGS[@]} -ge 4 ]; then
  # there are extra args after host/port/timeout -> exec them
  CMD=( "${ORIG_ARGS[@]:3}" )
  exec "${CMD[@]}"
else
  # default start command
  exec dotnet DesafioBlue.dll
fi
