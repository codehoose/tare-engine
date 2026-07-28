using TareEngine.Serialization;

namespace TareEditor.Services
{
    public enum ValidationSeverity
    {
        Warning,
        Error
    }

    public class ValidationIssue
    {
        public string RoomSlug { get; }
        public ValidationSeverity Severity { get; }
        public string Message { get; }

        public ValidationIssue(string roomSlug, ValidationSeverity severity, string message)
        {
            RoomSlug = roomSlug;
            Severity = severity;
            Message = message;
        }
    }

    public static class ProjectValidator
    {
        public static List<ValidationIssue> Validate(IReadOnlyList<SerializedRoom> rooms)
        {
            var issues = new List<ValidationIssue>();
            var bySlug = rooms
                .GroupBy(r => r.slug)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var group in bySlug.Values.Where(g => g.Count > 1))
            {
                issues.Add(new ValidationIssue(group[0].slug, ValidationSeverity.Error,
                    $"Duplicate slug '{group[0].slug}' is used by {group.Count} rooms."));
            }

            foreach (var room in rooms)
            {
                if (room.exits == null) continue;

                foreach (var (direction, targetSlug) in room.exits)
                {
                    if (!bySlug.TryGetValue(targetSlug, out var targets))
                    {
                        issues.Add(new ValidationIssue(room.slug, ValidationSeverity.Error,
                            $"Exit '{direction}' points to unknown room '{targetSlug}'."));
                        continue;
                    }

                    if (targetSlug == room.slug) continue; // self-loop: no reciprocity to check

                    var target = targets[0];
                    var hasReciprocalExit = target.exits != null && target.exits.Values.Contains(room.slug);
                    if (!hasReciprocalExit)
                    {
                        issues.Add(new ValidationIssue(room.slug, ValidationSeverity.Warning,
                            $"Exit '{direction}' to '{targetSlug}' has no exit back to '{room.slug}' (one-way)."));
                    }
                }
            }

            return issues;
        }
    }
}
