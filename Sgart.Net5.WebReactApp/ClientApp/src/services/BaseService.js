export class BaseService {
  async fetchGet(url) {
    return await this.fetchBase(url, FetchMethods.GET, false);
  }
  async fetchPost(url, jsonObject) {
    return await this.fetchBase(url, FetchMethods.POST, false, jsonObject);
  }
  async fetchPut(url, jsonObject) {
    return await this.fetchBase(url, FetchMethods.PUT, false, jsonObject);
  }
  async fetchDelete(url) {
    return await this.fetchBase(url, FetchMethods.DELETE, true);
  }

  async fetchBase(url, fetchMethod, addAuthorizationHeader, jsonObject) {
    var result = {
      ok: false,
      status: -1,
      message: null,
      data: null
    };

    try {
      const requestOptions = {
        headers: { 'Content-Type': 'application/json' },
        method: fetchMethod === undefined || fetchMethod === null || fetchMethod === '' ? 'GET' : fetchMethod,
      };
      //if (addAuthorizationHeader === true) {
      //  authService.setAuthorizationHeader(requestOptions);
      //}
      if (jsonObject) {
        requestOptions.body = JSON.stringify(jsonObject);
      }
      const response = await fetch(url, requestOptions);

      result.ok = response.ok;
      result.status = response.status;

      if (result.status === 200 || result.status === 201) {
        result.data = await response.json();
      } else if (result.status === 204) {
        result.data = null; //nessun contenuto
      } else if (result.status === 404) {
        result.message = '404 Page not found';
      } else if (result.status === 401) {
        result.message = '401 Unauthorized';
        //authService.setEmptyUser();
        //window.location.replace(AuthPaths.Login);
      } else if (result.status === 405) {
        result.message = '405 Method not allowed';
      } else {
        result.message = response.statusText === "" ? "**ERROR**" : response.statusText;
      }
    } catch (ex) {
      result.ok = false;
      console.error(`sgart:${ex.message}`, ex);
      if (ex.message === 'Failed to fetch') {
        result.message = 'Impossibile eseguire la fetch, verifica la connessione';
      } else {
        result.message = ex.message;
      }
    }
    if (result.ok === false || result.message !== null) {
      console.error(`sgart:baseService:url: ${url}, status: ${result.status}, message: ${result.message}`);
    }
    return result;
  }
}

const baseService = new BaseService();

export default baseService;

export const FetchMethods = {
  GET: 'GET',
  POST: 'POST',
  PUT: 'PUT',
  DELETE: 'DELETE'
};