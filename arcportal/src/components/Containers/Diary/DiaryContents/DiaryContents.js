import React from 'react';
import { ActivityItem, Icon, Link, mergeStyleSets } from '@fluentui/react';

const DiaryContents = (props) => {
    const classNames = mergeStyleSets({
        root: {
            marginTop: '20px',
        },
        nameText: {
            fontWeight: 'bold',
        },
    });

    const diaryEntries = [];
    if (props.data !== undefined) {
        for (const item of props.data) {
            const link = (
                <Link
                    key={item.Id}
                    className={classNames.nameText}
                    onClick={() => props.onClick(item.Id)}
                >
                    {item.LinkText}
                </Link>
            );
            const displayItem = {
                key: item.Id,
                activityIcon: <Icon iconName={item.Icon} />,
                activityDescription: [
                    <span key={1}>{item.StartText} </span>,
                    link,
                    <span key={2}>{item.EndText}</span>,
                ],
                timeStamp: item.Timestamp,
            };
            diaryEntries.push(displayItem);
        }
    }

    return (
        <div>
            {diaryEntries.map((item) => (
                <ActivityItem
                    {...item}
                    key={item.key}
                    className={classNames.root}
                />
            ))}
        </div>
    );
};

export default DiaryContents;
