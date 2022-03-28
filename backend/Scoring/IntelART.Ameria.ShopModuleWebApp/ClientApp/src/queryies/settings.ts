import Api from 'services/api';

const path = `Settings`;

export const getFileMaxSize = async () => {
    const { data } = await Api.get(`${path}/FileMaxSize`);
    return data;
};
