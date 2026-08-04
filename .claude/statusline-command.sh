#!/usr/bin/env bash
# Claude Code Status Line — 4 segments with color-coded progress bars
# Input: JSON via stdin from Claude Code
input=$(cat)

# --- Segment 1: Current Working Directory ---
cwd=$(echo "$input" | jq -r '.workspace.current_dir // empty')
if [ -n "$cwd" ]; then
  printf '\033[1;36m/%s\033[0m' "$cwd"
fi

# --- Segment 2: Model Name + Thinking Effort ---
model=$(echo "$input" | jq -r '.model.display_name // .model.id // empty')
effort=$(echo "$input" | jq -r '.effort.level // empty')
thinking=$(echo "$input" | jq -r '.thinking.enabled // false')
if [ -n "$model" ]; then
  printf ' | \033[1;33m%s\033[0m' "$model"
  if [ "$thinking" = "true" ] && [ -n "$effort" ]; then
    printf ' [\033[1;35m%s\033[0m]' "$effort"
  fi
fi

# --- Segment 3: Context Window Usage with Color-Coded Progress Bar ---
used=$(echo "$input" | jq -r '.context_window.used_percentage // empty')
if [ -n "$used" ]; then
  pct=$(printf '%.0f' "$used")
  # Choose color based on usage
  if [ "$pct" -le 30 ]; then
    color="32"  # Green
  elif [ "$pct" -le 70 ]; then
    color="33"  # Yellow
  else
    color="31"  # Red
  fi
  # Build progress bar (20 chars wide)
  bar=""
  i=0
  while [ $i -lt 20 ]; do
    if [ $i -lt $((pct * 20 / 100)) ]; then
      bar="${bar}█"
    else
      bar="${bar}░"
    fi
    i=$((i + 1))
  done
  printf ' | ctx[\033[%sm%s\033[0m] \033[%sm%s%%\033[0m' "$color" "$bar" "$color" "$pct"
fi

# --- Segment 4: 5-Hour Session Limit ---
five=$(echo "$input" | jq -r '.rate_limits.five_hour.used_percentage // empty')
if [ -n "$five" ]; then
  pct5=$(printf '%.0f' "$five")
  # Color for 5-hour limit
  if [ "$pct5" -le 30 ]; then
    color5="32"
  elif [ "$pct5" -le 70 ]; then
    color5="33"
  else
    color5="31"
  fi
  # Calculate resets_at as human-readable time remaining
  resets=$(echo "$input" | jq -r '.rate_limits.five_hour.resets_at // empty')
  remaining=""
  if [ -n "$resets" ]; then
    now=$(date +%s)
    diff=$((resets - now))
    if [ "$diff" -gt 0 ]; then
      hrs=$((diff / 3600))
      mins=$(( (diff % 3600) / 60 ))
      remaining=" (resets in ${hrs}h${mins}m)"
    fi
  fi
  bar5=""
  i=0
  while [ $i -lt 20 ]; do
    if [ $i -lt $((pct5 * 20 / 100)) ]; then
      bar5="${bar5}█"
    else
      bar5="${bar5}░"
    fi
    i=$((i + 1))
  done
  printf ' | 5h[\033[%sm%s\033[0m] \033[%sm%s%%\033[0m' "$color5" "$bar5" "$color5" "$pct5"
  [ -n "$remaining" ] && printf '\033[90m%s\033[0m' "$remaining"
fi

echo