import React, { useState, useEffect, useCallback } from 'react';
import './ReportContainer.css';
import ReportDesigner from './ReportDesigner';
import Post from '../../../Data/Post';
import axios from 'axios';
import { IconButton, TooltipHost, Separator } from '@fluentui/react';
import TranslateTag from '../../../Utils/Local/TranslateTag';

const ReportContainer = (props) => {
    // State for fetched report configuration
    const [fetchedReportConfig, setFetchedReportConfig] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [availableImages, setAvailableImages] = useState([]);

    // Extract config fetching logic into reusable function
    const fetchReportConfig = useCallback(() => {
        if (!props.config?.id) {
            return;
        }

        const [id, name] = props.config.id.split("|");
        setIsLoading(true);
        setError(null);

        const successFunction = (data) => {
            const parsedData = JSON.parse(data);
            setFetchedReportConfig(parsedData);
            setIsLoading(false);
        };

        const errorFunction = (error) => {
            setError(error.message || 'Failed to fetch report configuration');
            setIsLoading(false);
        };

        Post('config/getreportdesignerconfig', { id: id, name: name }, successFunction, errorFunction);
    }, [props.config?.id]);

    // Fetch report designer configuration on component load
    useEffect(() => {
        fetchReportConfig();
    }, [fetchReportConfig]);

    // Fetch available images on component mount
    useEffect(() => {
        const fetchAvailableImages = async () => {
            try {
                const token = localStorage.getItem('arctoken');
                const response = await axios.get('image', {
                    headers: token ? { Authorization: `Bearer ${token}` } : {}
                });
                
                // Map backend response to expected format
                const images = (response.data || []).map(img => ({
                    name: img.name || '',
                    Description: img.description || '',
                    FileAttachmentId: img.fileAttachmentId
                }));
                
                setAvailableImages(images);
            } catch (error) {
                // Set empty array to prevent crashes
                setAvailableImages([]);
            }
        };

        fetchAvailableImages();
    }, []);


    /**
     * Posts the designer change set and hands the save result back to the designer.
     *
     * The result describes what the backend actually stored, including any record it had to rename to
     * avoid a name collision, so the designer can reconcile before the refetch lands.
     * @param {object} reportConfig - The SaveReportDesignerConfigModel payload.
     * @returns {Promise<object|null>} The parsed ReportDesignerSaveResultModel, or null if unparseable.
     */
    const handleSaveReport = (reportConfig) => {
        // Return a Promise so callers can await HTTP 200 before proceeding
        return new Promise((resolve, reject) => {
            const successFunction = (data) => {
                let saveResult = null;
                try {
                    saveResult = typeof data === 'string' ? JSON.parse(data) : data;
                } catch (parseError) {
                    saveResult = null;
                }

                // Silently reload the configuration from backend to get updated names
                fetchReportConfig();
                // Call the parent callback if provided (without passing response message)
                if (props.onReportSave) {
                    props.onReportSave(reportConfig);
                }
                resolve(saveResult);
            };

            const errorFunction = (error) => {
                reject(error);
            };

            Post('config/savereportdesignerconfig', reportConfig, successFunction, errorFunction);
        });
    };

    const handleReportChange = (reportConfig, hasUnsavedChanges) => {
        // You can use this callback to:
        // 1. Track unsaved changes in your parent component
        // 2. Show unsaved changes indicators
        // 3. Auto-save functionality
        // 4. Warn users before navigation
        
        // Call parent callback if provided
        if (props.onReportChange) {
            props.onReportChange(reportConfig, hasUnsavedChanges);
        }
    };

    const handleBackNavigation = () => {
        if (props.onBack) {
            props.onBack();
        } else if (props.cancel) {
            props.cancel();
        }
    };

    return (
        <div className="report-container">
            <div className='managerecord-top-buttons'>
                <div className='managerecord-titlebar-left'>
                    <div className='managerecord-button'>
                        <TooltipHost content={TranslateTag("@GenExi@", props.language)}>
                            <IconButton
                                iconProps={{iconName: 'Back'}}
                                onClick={handleBackNavigation}
                            />
                        </TooltipHost>
                    </div>
                    <div className='managerecord-title'>
                        Report Designer
                    </div>
                </div>
            </div>
            <div className='managerecord-topbar-separator'>
                <Separator></Separator>
            </div>
            {isLoading && <div>Loading report configuration...</div>}
            {error && <div style={{color: 'red'}}>Error loading configuration: {error}</div>}
            <div className="report-container-content">
                <ReportDesigner
                    reportConfig={fetchedReportConfig}
                    availableImages={availableImages}
                    onSave={handleSaveReport}
                    onReportChange={handleReportChange}
                    {...props} 
                />
            </div>
        </div>
    );
};

export default ReportContainer;
