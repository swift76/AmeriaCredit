import React, { useEffect, useState } from 'react';
import { Form, Button, ProgressBar } from 'react-bootstrap';
import { useFormContext } from 'react-hook-form';
import { useFileMaxSize } from 'hooks';
import clsx from 'clsx';
import { openFile } from 'helpers/data';
import API from 'services/api';

type Props = {
    label: string | React.ReactNode;
    url: string;
    uploaded: boolean;
    name: string;
};

export default function Uploader({ label, url, uploaded = false, name }: Props) {
    const [error, setError] = useState<any>(null);
    const { register, setValue, errors } = useFormContext();
    const [isUploaded, setIsUploaded] = useState(false);
    const [showProgress, setShowProgress] = useState(false);
    const [progress, setProgressPercent] = useState<number>(0);
    const { data: fileMaxSize = 0 } = useFileMaxSize();

    useEffect(() => {
        if (errors[name]) {
            setError(errors[name].message);
        }
    }, [name, errors]);

    const handleDownloadFile = async () => {
        openFile(url);
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setIsUploaded(false);
        const { files } = event.target;
        const formData = new FormData();
        if (files && files.length) {
            const file = files[0];
            if (file.size > fileMaxSize) {
                return setError(
                    `Ֆայլի չափը չպետք է գերազանցի ${Math.ceil(fileMaxSize / 1024)}-կբ ը`
                );
            }
            formData.append('file', file);
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onloadend = async () => {
                API.put(window.location.origin + url, formData, {
                    onUploadProgress: pevent => {
                        setShowProgress(true);
                        const percent = Math.floor((pevent.loaded * 100) / pevent.total);
                        setProgressPercent(percent);
                    }
                }).then(() => {
                    setIsUploaded(true);
                    setValue(name, true, { shouldValidate: true });
                    setProgressPercent(0);
                    setShowProgress(false);
                });
            };
        }
        return true;
    };

    useEffect(() => {
        register(name);
    }, [register, name]);

    useEffect(() => {
        if (uploaded) {
            setIsUploaded(true);
            setValue(name, true);
        }
    }, [name, setValue, uploaded]);

    return (
        <div className={clsx(`file-uploader`, isUploaded && 'is-uploaded')}>
            <Form.File custom>
                <Form.File.Input
                    isInvalid={!!error}
                    onChange={handleChange}
                    name={name}
                    accept="image/x-png,image/gif,image/jpeg"
                />
                <Form.File.Label data-browse="Կցել">{label}</Form.File.Label>
                {error && <Form.Control.Feedback type="invalid">{error}</Form.Control.Feedback>}
                {showProgress && <ProgressBar animated now={progress} label={`${progress}%`} />}
            </Form.File>
            {isUploaded && (
                <Button variant="primary" className="ml-4" onClick={handleDownloadFile}>
                    Բեռնել
                </Button>
            )}
        </div>
    );
}
