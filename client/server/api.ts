const BASE_URL = "https://localhost:7064/api";


export async function fetchGetApi<T>(path: string): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return (await response.json()) as T;
}

export async function fetchGetByIdApi<T>(id: any, path: string): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}/${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return (await response.json()) as T;
}


export async function fetchPostApi<T>(path: string, body: any): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body:  JSON.stringify(body),
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return (await response.json()) as T;
}

export async function fetchPutApi<T>(id:any, path: string, body: any): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  });

  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return (await response.json()) as T;
}

export async function fetchDeleteApi<T>(id: any, path: string): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}/${id}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  });
  
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return (await response.json()) as T;
}