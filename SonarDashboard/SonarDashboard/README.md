# SonarDashboard

This package contains the starter structure for an offline SonarQube AI Dashboard.

## You need to add these local libraries once:

css/
- bootstrap.min.css
- bootstrap-icons.css
- dataTables.bootstrap5.min.css

js/
- jquery.min.js
- bootstrap.bundle.min.js
- chart.umd.min.js
- dataTables.min.js
- dataTables.bootstrap5.min.js

The HTML template uses placeholders like:
{{ExecutiveSummary}}
{{TopFiles}}
{{Recommendations}}

Replace these placeholders from your HtmlRenderAgent.
