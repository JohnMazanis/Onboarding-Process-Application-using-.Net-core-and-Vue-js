import axios from 'axios'

const client = axios.create({
    baseURL: 'api/controller',
    json: true
});

export default {
    async execute(method, resource, data) {
        return client({
            method,
            url: resource,
            data
        }).then(req => {
            return req.data
        })
    },
    GetRequest(data, functionName) {
        return this.execute('get', `${functionName}/${data}`, data)
    },
    PostRequest(data, functionName) {
        return this.execute('post', `${functionName}/${data}`, data)
    }
}