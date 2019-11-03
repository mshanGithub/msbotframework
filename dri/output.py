# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.
from colorama import Fore, Style, init
from datetime import datetime, timedelta
from helpers import strfdelta
import jsonpickle

# Initialize colorama
init(convert=True)
file_name = "botreport_" + datetime.now().strftime("%Y%m%d_%H%M%S") + ".html"
output_file = open(file_name, mode="w", encoding="utf-8")

class OuputIssuesJson():
    def __init__(self):
        self.repositories = []

    def write_output(self, file_name = "botreport_" + datetime.now().strftime("%Y%m%d_%H%M%S") + ".json"):
        with open(file_name, mode="w", encoding="utf-8") as json_file:
            json_file.write(jsonpickle.encode(self, unpicklable=False))

class OutputRepository():
    def __init__(self, name):
        self.name = name
        self.issues = []

class OutputIssue():
    def __init__(self, tag, issue):
        self.tag = tag
        self.issue = issue

def print_issue(issue):
    print(f'        {issue.number} : {issue.title}')
    print(f'             {issue.html_url}')
    
    output_file.write(f'<span class="tab2">{issue.number} : <a href="{issue.html_url}" target="_blank">{issue.title}</a></span>')
    output_file.write("<br/>")
    
    # Uncomment if you want to add labels.
    # add_label(repo, issue, BOT_SERVICES_LABEL)

def print_status(text, css=''):
    print(u''+text)
    has_css = True if (len(css) > 0) else False
    if (has_css):
        output_file.write(f"<span class='{css}'>")
    output_file.write(f"{text}</br>")
    if (has_css):
        output_file.write("</span>")


def print_stale_issue(issue):
    print(f'         {issue.number} : {issue.title}')
    output_file.write(f'<span class="tab2">{issue.number} : <a href="{issue.html_url}" target="_blank">{issue.title}</a></span>')
    output_file.write("<br/>")
    print_status(f'            Issue Age: {Fore.RED}{strfdelta(datetime.utcnow() - issue.created_at, "{days} days {hours}:{minutes}:{seconds}")}{Style.RESET_ALL}','tab3')
    print_status(f'            Last Comment: {Fore.RED}{strfdelta(datetime.utcnow() - issue.last_comment, "{days} days {hours}:{minutes}:{seconds}")}{Style.RESET_ALL}','tab3')
    print(f'                 {issue.html_url}')

def print_break():
    output_file.write("<br/>")

def setup_html():
    output_file.write("<html>\n<head>\n<title>Bot Report</title>\n")
    output_file.write("<style type='text/css'>body{ font-family: Helvetica,Arial,sans-serif; }.tab1 { margin-left: 30px; }.tab2 { margin-left: 60px; }.tab3 { margin-left: 90px; }</style>\n")
    output_file.write("</head>\n<body>\n")
    return output_file