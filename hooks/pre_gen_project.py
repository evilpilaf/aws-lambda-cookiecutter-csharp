import re
import sys

SLUG_REGEX = r'^[a-zA-Z][_a-zA-Z0-9]+$'
slug_name = '{{ cookiecutter.project_slug }}'

APPGROUP_REGEX = r'^[a-z0-9][_a-z0-9]{1,22}$'
appgroup = '{{ cookiecutter.project_app_group }}'

if not re.match(APPGROUP_REGEX, appgroup):
    print('ERROR: \'%s\' is not a valid value for the app group' % appgroup)
    sys.exit(1)

if not re.match(SLUG_REGEX, slug_name):
    print('ERROR: \'%s\' is not a valid namespace value.' % slug_name)
    sys.exit(1)
