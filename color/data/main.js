[{"kind":"comment","value":"prelude"},{"kind":"comment","value":"use"},{"kind":"symbol","value":"turtle"},{"kind":"immediate","value":"use"},{"kind":"symbol","value":"core"},{"kind":"immediate","value":"use"},{"kind":"editor","value":"↵"},{"kind":"define","value":"circle"},{"kind":"literal","value":"200"},{"kind":"define","value":"loop"},{"kind":"compile","value":"dup"},{"kind":"literal","value":"0"},{"kind":"compile","value":"≥"},{"kind":"immediate","value":"if"},{"kind":"literal","value":"1"},{"kind":"compile","value":"-"},{"kind":"literal","value":"0.0314"},{"kind":"compile","value":"right"},{"kind":"literal","value":"2"},{"kind":"compile","value":"forward"},{"kind":"immediate","value":"then"},{"kind":"compile","value":"loop"},{"kind":"compile","value":";"},{"kind":"compile","value":"drop"},{"kind":"compile","value":";"},{"kind":"editor","value":"↵"},{"kind":"define","value":"spiral"},{"kind":"literal","value":"20"},{"kind":"define","value":"loop"},{"kind":"compile","value":"dup"},{"kind":"literal","value":"0"},{"kind":"compile","value":"≥"},{"kind":"immediate","value":"if"},{"kind":"compile","value":"circle"},{"kind":"literal","value":"1"},{"kind":"compile","value":"-"},{"kind":"literal","value":"0.314"},{"kind":"compile","value":"right"},{"kind":"literal","value":"2"},{"kind":"immediate","value":"then"},{"kind":"compile","value":"loop"},{"kind":"compile","value":";"},{"kind":"compile","value":"drop"},{"kind":"compile","value":";"},{"kind":"immediate","value":"spiral"}]