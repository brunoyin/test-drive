#!/usr/bin/env python
# coding: utf-8

import os
import sys
import json
import uuid
import tempfile
from flask import Flask, request, Response, g

app = Flask(__name__)
app.debug = True


def save_request(uuid, request):
    req_data = {}
    req_data['uuid'] = uuid
    req_data['endpoint'] = request.endpoint
    req_data['method'] = request.method
    req_data['cookies'] = request.cookies
    # req_data['data'] = request.data
    if 'Content-Type' in request.headers and request.headers['Content-Type'].startswith('application/json'):
        req_data['data'] = request.json
    req_data['headers'] = dict(request.headers)
    req_data['headers'].pop('Cookie', None)
    req_data['args'] = request.args
    req_data['form'] = request.form
    req_data['remote_addr'] = request.remote_addr
    if request.files is not None:
        files = []
        for name, fs in request.files.items():
            dst = tempfile.NamedTemporaryFile()
            fs.save(dst)
            dst.flush()
            filesize = os.stat(dst.name).st_size
            dst.close()
            files.append({'name': name, 'filename': fs.filename, 'filesize': filesize,
             'mimetype': fs.mimetype, 'mimetype_params': fs.mimetype_params})
        req_data['files'] = files
    return req_data


def save_response(uuid, resp):
    resp_data = {}
    resp_data['uuid'] = uuid
    resp_data['status_code'] = resp.status_code
    resp_data['status'] = resp.status
    resp_data['headers'] = dict(resp.headers)
    # resp_data['data'] = resp.response
    return resp_data


@app.before_request
def before_request():
    print (request.method, request.endpoint)


@app.after_request
def after_request(resp):
    resp.headers.add('Access-Control-Allow-Origin', '*')
    resp.headers.add('Access-Control-Allow-Headers', 'Content-Type, X-Token')
    resp.headers.add('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE')
    resp_data = save_response(g.uuid, resp)
    print ('Response:: ', json.dumps(resp_data, indent=4))
    return resp


@app.route('/log', methods=['GET', 'POST', 'DELETE', 'PUT'])
def log():
    g.uuid = uuid.uuid1().hex
    req_data = save_request(g.uuid, request)
    resp = Response(json.dumps(req_data, indent=4), mimetype='application/json')
    resp.set_cookie('cookie-name', value='cookie-value')
    return resp


if __name__ == '__main__':
    app.run(debug=True)