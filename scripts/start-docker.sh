#!/usr/bin/env bash
set -euo pipefail

# scripts/start-docker.sh
# Helper to build and run the application stack with docker compose and follow logs.
# Usage:
#   ./scripts/start-docker.sh            # build, start (detached) and follow app logs
#   ./scripts/start-docker.sh up         # same as above
#   ./scripts/start-docker.sh down       # stop and remove containers + volumes
#   ./scripts/start-docker.sh rebuild    # down + up --build
#   ./scripts/start-docker.sh logs       # follow app logs only

ROOT_DIR=$(cd "$(dirname "$0")/.." && pwd)
COMPOSE_FILE="$ROOT_DIR/docker-compose.yml"

if ! command -v docker >/dev/null 2>&1; then
  echo "docker is not installed or not in PATH" >&2
  exit 1
fi

if ! docker compose version >/dev/null 2>&1; then
  echo "docker compose not available. Use Docker Compose v2 (docker compose) or install it." >&2
  exit 1
fi

CMD=${1:-up}

case "$CMD" in
  up)
    echo "Starting stack (build if needed)..."
    docker compose -f "$COMPOSE_FILE" up --build -d
    echo "Following logs for 'app' service (press Ctrl+C to detach)..."
    docker compose -f "$COMPOSE_FILE" logs -f app
    ;;

  rebuild)
    echo "Rebuilding: tearing down existing stack, then building and starting..."
    docker compose -f "$COMPOSE_FILE" down -v
    docker compose -f "$COMPOSE_FILE" up --build -d
    docker compose -f "$COMPOSE_FILE" logs -f app
    ;;

  down)
    echo "Stopping and removing containers, networks and volumes..."
    docker compose -f "$COMPOSE_FILE" down -v
    ;;

  logs)
    echo "Following logs for 'app' service..."
    docker compose -f "$COMPOSE_FILE" logs -f app
    ;;

  status)
    docker compose -f "$COMPOSE_FILE" ps
    ;;

  *)
    echo "Unknown command: $CMD" >&2
    echo "Usage: $0 [up|rebuild|down|logs|status]"
    exit 2
    ;;
esac
